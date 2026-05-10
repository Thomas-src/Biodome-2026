using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Infrastructures.entities;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
[ValidateAntiForgeryToken]
public class EditModel : PageModel
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUserStore<UserEntity> _userStore;
    private readonly ILogger<EditModel> _logger;
    private readonly IResearchService _researchOrganismService;
    private readonly IPictureRepository _pictureRepository;
    private readonly SignInManager<UserEntity> _signInManager;

    public EditModel(
        UserManager<UserEntity> userManager,
        IUserStore<UserEntity> userStore,
        ILogger<EditModel> logger,
        IResearchService researchOrganismService,
        IPictureRepository pictureRepository,
        SignInManager<UserEntity> signInManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _researchOrganismService = researchOrganismService;
        _pictureRepository = pictureRepository;
        _signInManager = signInManager;
    }

    public IReadOnlyList<ResearchOrganism> Organismes { get; set; } = [];
    public string? CurrentAvatar { get; set; }
    public string? Email { get; set; }

    [BindProperty] public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return RedirectToPage("/Account/Login");

        LoadOrganismes();

        CurrentAvatar = user.Avatar;
        Email = user.Email;

        Input = new InputModel
        {
            Nom = user.UserName ?? string.Empty,
            DateNaissance = user.DateOfBirth,
            ResearchOrganism = user.ResearchOrganismId,
            SelectedAvatar = user.Avatar
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        LoadOrganismes();

        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentAvatar = user.Avatar;

        ValidateInputs();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await UpdateUserProfileAsync(user);
        await UpdateAvatarAsync(user);
        await UpdateAvatarClaimAsync(user);

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            AddErrorsToModelState(updateResult.Errors);
            return Page();
        }

        await UpdatePasswordIfNeededAsync(user);
        await _signInManager.RefreshSignInAsync(user);

        _logger.LogInformation("Profil mis à jour pour {Email}", user.Email);
        TempData["Success"] = "Profil mis à jour avec succès.";

        return RedirectToPage();
    }

    private async Task<UserEntity?> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }

    private void ValidateInputs()
    {
        CheckInputs();
    }

    private async Task UpdateUserProfileAsync(UserEntity user)
    {
        await _userStore.SetUserNameAsync(user, Input.Nom, CancellationToken.None);

        user.DateOfBirth = Input.DateNaissance;
        user.ResearchOrganismId = Input.ResearchOrganism;
    }

    private async Task UpdateAvatarAsync(UserEntity user)
    {
        if (Input.Avatar is not null)
        {
            var filename = _pictureRepository.Save(
                Input.Avatar.FileName,
                Input.Avatar.OpenReadStream(),
                "avatars");

            user.Avatar = filename;

            _logger.LogInformation(
                "Avatar mis à jour via upload: {Filename}",
                filename);

            return;
        }

        if (!string.IsNullOrWhiteSpace(Input.SelectedAvatar))
        {
            user.Avatar = Input.SelectedAvatar;

            _logger.LogInformation(
                "Avatar mis à jour via sélection: {SelectedAvatar}",
                Input.SelectedAvatar);
        }
    }

    private async Task UpdateAvatarClaimAsync(UserEntity user)
    {
        if (Input.Avatar is null && string.IsNullOrWhiteSpace(Input.SelectedAvatar))
        {
            return;
        }

        var existingClaims = await _userManager.GetClaimsAsync(user);
        var avatarClaim = existingClaims.FirstOrDefault(c => c.Type == "Avatar");

        if (avatarClaim is not null)
        {
            await _userManager.RemoveClaimAsync(user, avatarClaim);
        }

        await _userManager.AddClaimAsync(
            user,
            new Claim("Avatar", user.Avatar ?? "1.png"));
    }

    private async Task UpdatePasswordIfNeededAsync(UserEntity user)
    {
        if (string.IsNullOrWhiteSpace(Input.Password))
        {
            return;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(
            user,
            token,
            Input.Password);

        if (!result.Succeeded)
        {
            AddErrorsToModelState(result.Errors);
        }
    }

    private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Votre nom")]
        [RegularExpression("@”^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisé")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        [RegularExpression("@”^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisé")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [RegularExpression("@”^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisé")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateOnly DateNaissance { get; set; }

        [Display(Name = "Avatar")]
        [IsImageValid]
        public IFormFile? Avatar { get; set; }

        public string? SelectedAvatar { get; set; }

        [Display(Name = "Organisme de recherche")]
        public string? ResearchOrganism { get; set; }
    }

    private IUserEmailStore<UserEntity> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("User store must support email.");
        }


        return (IUserEmailStore<UserEntity>)_userStore;
    }

    private void CheckInputs()
    {
        if (Input.DateNaissance > DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
        {
            ModelState.AddModelError("Input.DateNaissance", "Vous devez avoir au moins 18 ans.");
        }

        if (!string.IsNullOrWhiteSpace(Input.Password) && Input.Password != Input.ConfirmPassword)
        {
            ModelState.AddModelError("Input.ConfirmPassword", "Les mots de passe ne correspondent pas.");
        }
    }

    private void LoadOrganismes() => Organismes = _researchOrganismService.GetAllOrganisms();
}
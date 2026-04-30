using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;

[Authorize]
public class EditModel : PageModel
{
    private readonly UserManager<Infrastructures.entities.UserEntity> _userManager;
    private readonly IUserStore<Infrastructures.entities.UserEntity> _userStore;
    private readonly ILogger<EditModel> _logger;
    private readonly IResearchService _researchOrganismService;
    private readonly IUserEmailStore<Infrastructures.entities.UserEntity> _emailStore;
    private readonly IPictureRepository _pictureRepository;

    public EditModel(
        UserManager<Infrastructures.entities.UserEntity> userManager,
        IUserStore<Infrastructures.entities.UserEntity> userStore,
        ILogger<EditModel> logger,
        IResearchService researchOrganismService,
        IPictureRepository pictureRepository)
    {
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _emailStore = GetEmailStore();
        _researchOrganismService = researchOrganismService;
        _pictureRepository = pictureRepository;
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

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToPage("/Account/Login");
        }

        CurrentAvatar = user.Avatar;

        if (Input.Avatar is not null)
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

            if (!allowed.Contains(Input.Avatar.ContentType))
            {
                ModelState.AddModelError("Input.Avatar", "Seuls les formats JPG, PNG, GIF et WebP sont acceptés.");
            }

            if (Input.Avatar.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Input.Avatar", "L'avatar ne peut pas dépasser 2 Mo.");
            }
        }

        if (Input.DateNaissance > DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
        {
            ModelState.AddModelError("Input.DateNaissance", "Vous devez avoir au moins 18 ans.");
        }

        if (!string.IsNullOrWhiteSpace(Input.Password) && Input.Password != Input.ConfirmPassword)
        {
            ModelState.AddModelError("Input.ConfirmPassword", "Les mots de passe ne correspondent pas.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _userStore.SetUserNameAsync(user, Input.Nom, CancellationToken.None);

        user.DateOfBirth = Input.DateNaissance;
        user.ResearchOrganismId = Input.ResearchOrganism;

        if (Input.Avatar is not null)
        {
            var filename = _pictureRepository.Save(Input.Avatar.FileName, Input.Avatar.OpenReadStream(), "avatars");
            user.Avatar = filename;
            _logger.LogInformation("Avatar mis à jour via upload: {f}", filename);
        }
        else if (!string.IsNullOrEmpty(Input.SelectedAvatar))
        {
            user.Avatar = Input.SelectedAvatar;
            _logger.LogInformation("Avatar mis à jour via sélection: {s}", Input.SelectedAvatar);
        }

        _logger.LogInformation("BEFORE UPDATE Avatar = {a}", user.Avatar);
        var result = await _userManager.UpdateAsync(user);
        _logger.LogInformation("AFTER UPDATE SUCCESS = {s}", result.Succeeded);

        if (result.Succeeded && !string.IsNullOrWhiteSpace(Input.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var pwResult = await _userManager.ResetPasswordAsync(user, token, Input.Password);

            if (!pwResult.Succeeded)
            {
                foreach (var error in pwResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        _logger.LogInformation("Profil mis à jour pour {Email}", user.Email);
        TempData["Success"] = "Profil mis à jour avec succès.";

        return RedirectToPage();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Votre nom")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateOnly DateNaissance { get; set; }

        [Display(Name = "Avatar")] public IFormFile? Avatar { get; set; }

        public string? SelectedAvatar { get; set; }

        [Display(Name = "Organisme de recherche")]
        public string? ResearchOrganism { get; set; }
    }

    private IUserEmailStore<Infrastructures.entities.UserEntity> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("User store must support email.");
        }


        return (IUserEmailStore<Infrastructures.entities.UserEntity>)_userStore;
    }

    private void LoadOrganismes()
        => Organismes = _researchOrganismService.GetAllOrganisms();
}
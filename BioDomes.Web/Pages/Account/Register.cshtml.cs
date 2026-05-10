using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Domains.Services.mail;
using BioDomes.Infrastructures.entities;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BioDomes.Web.Pages.Account;

[ValidateAntiForgeryToken]
public class RegisterModel : PageModel
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUserStore<UserEntity> _userStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IResearchService _researchOrganismService;
    private readonly IUserEmailStore<UserEntity> _emailStore;
    private readonly IMyEmailSender _emailSender;
    private readonly IPictureRepository _pictureRepository;

    public RegisterModel(
        UserManager<UserEntity> userManager,
        IUserStore<UserEntity> userStore,
        ILogger<RegisterModel> logger,
        IResearchService researchOrganismService,
        IMyEmailSender emailSender,
        IPictureRepository pictureRepository)
    {
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _emailStore = GetEmailStore();
        _researchOrganismService = researchOrganismService;
        _emailSender = emailSender;
        _pictureRepository = pictureRepository;
    }

    public IReadOnlyList<ResearchOrganism> Organismes { get; set; } = [];

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/";
        LoadOrganismes();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        InitializePage(returnUrl);
        LoadOrganismes();
        ValidateInputs();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (await EmailAlreadyExistsAsync())
        {
            return Page();
        }
        var user = await BuildUserAsync();
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var creationResult = await _userManager.CreateAsync(user, Input.Password);

        if (!creationResult.Succeeded)
        {
            HandleCreationErrors(creationResult.Errors);
            return Page();
        }
        await FinalizeUserRegistrationAsync(user);
        return RedirectToPage("/Account/RegisterSuccess", new { email = Input.Email });
    }

    private void InitializePage(string? returnUrl)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    private void ValidateInputs()
    {
        CheckInputs();
    }

    private async Task<bool> EmailAlreadyExistsAsync()
    {
        var existingUser = await _userManager.FindByEmailAsync(Input.Email);
        if (existingUser is null)
        {
            return false;
        }
        ModelState.AddModelError("Input.Email", "Cette adresse e-mail est déjà utilisée.");
        return true;
    }

    private async Task<UserEntity> BuildUserAsync()
    {
        var user = CreateUser();
        await _userStore.SetUserNameAsync(user, Input.Nom, CancellationToken.None);

        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        user.DateOfBirth = Input.DateNaissance;
        user.ResearchOrganismId = Input.ResearchOrganism;
        AssignAvatar(user);

        return user;
    }

    private void AssignAvatar(UserEntity user)
    {
        if (Input.Avatar is not null)
        {
            var filename = _pictureRepository.Save(
                Input.Avatar.FileName,
                Input.Avatar.OpenReadStream(),
                "avatars");

            user.Avatar = filename;
            return;
        }

        if (string.IsNullOrWhiteSpace(Input.SelectedAvatar))
        {
            return;
        }

        if (!IsValidAvatar(Input.SelectedAvatar))
        {
            ModelState.AddModelError(
                "Input.SelectedAvatar",
                "Avatar invalide.");

            return;
        }

        user.Avatar = Input.SelectedAvatar;
    }

    private bool IsValidAvatar(string selectedAvatar)
    {
        var allowedAvatars = new[]
        {
            "1.png",
            "2.png",
            "3.png",
            "4.png"
        };

        return allowedAvatars.Contains(selectedAvatar);
    }

    private async Task FinalizeUserRegistrationAsync(UserEntity user)
    {
        await _userManager.AddClaimAsync(user, new Claim("Avatar", user.Avatar ?? "1.png"));
        await _userManager.AddToRoleAsync(user, AppRoles.Research);
        _logger.LogInformation("Nouvel utilisateur créé : {Email}", Input.Email);

        await SendConfirmationEmailAsync(user);
    }

    private async Task SendConfirmationEmailAsync(UserEntity user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        _logger.LogInformation("Token encodé de l'utilisateur {Token}", encodedToken);

        var confirmationLink = Url.Page("/Account/ConfirmEmail", pageHandler: null,
            values: new
            {
                userId = user.Id,
                token = encodedToken
            },
            protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(
            Input.Email,
            "Confirme ton compte BioDomes",
            $@"
<h2>Bienvenue {Input.Nom}</h2>
<p>Clique sur le lien pour activer ton compte :</p>
<a href='{confirmationLink}'>Confirmer mon compte</a>
");
    }

    private void HandleCreationErrors(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
        {
            _logger.LogError(error.Description);

            ModelState.AddModelError(string.Empty, "Une erreur s'est produite");
        }
    }

    [BindProperty] public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Le nom doit contenir entre 5 et 50 caractères.")]
        [Display(Name = "Nom du chercheur")]
        [RegularExpression(@"^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisés")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Avatar prédéfini")] 
        public string? SelectedAvatar { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [IsEmailValid]
        [Display(Name = "Email")]
        [RegularExpression(@"^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisés")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [RegularExpression(@"^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisés")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [RegularExpression(@"^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisés")]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        [RegularExpression(@"^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisés")]
        public required DateOnly DateNaissance { get; set; }

        [Display(Name = "Avatar")]
        [IsImageValid]
        public IFormFile? Avatar { get; set; }

        [Required(ErrorMessage = "Veuillez choisir un organisme de recherche.")]
        [Display(Name = "Organisme de recherche")]
        public required string ResearchOrganism { get; set; }
    }

    private IUserEmailStore<UserEntity> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<UserEntity>)_userStore;
    }

    private void LoadOrganismes()
    {
        Organismes = _researchOrganismService.GetAllOrganisms();
    }

    private UserEntity? CreateUser()
    {
        try
        {
            return Activator.CreateInstance<UserEntity>();
        }
        catch
        {
            ModelState.AddModelError("", "Une erreur s'est produite");
        }

        return null;
    }

    private void CheckInputs()
    {
        if (Input.DateNaissance > DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
        {
            ModelState.AddModelError("Input.DateNaissance", "Vous devez avoir au moins 18 ans pour vous inscrire.");
        }

        if (Input.Avatar is null && string.IsNullOrWhiteSpace(Input.SelectedAvatar))
        {
            ModelState.AddModelError("Input.SelectedAvatar", "Veuillez sélectionner ou uploader un avatar.");
        }
    }
}
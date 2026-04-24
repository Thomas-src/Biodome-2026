using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.mail;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<Infrastructures.entities.UserEntity> _userManager;
    private readonly IUserStore<Infrastructures.entities.UserEntity> _userStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IResearchService _researchOrganismService;
    private readonly SignInManager<Infrastructures.entities.UserEntity> _signInManager;
    private readonly IUserEmailStore<Infrastructures.entities.UserEntity> _emailStore;
    private readonly IMyEmailSender _emailSender;

    public RegisterModel(
        UserManager<Infrastructures.entities.UserEntity> userManager,
        IUserStore<Infrastructures.entities.UserEntity> userStore,
        ILogger<RegisterModel> logger,
        IResearchService researchOrganismService,
        SignInManager<Infrastructures.entities.UserEntity> signInManager,
        IMyEmailSender emailSender)
    {
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _emailStore = GetEmailStore();
        _researchOrganismService = researchOrganismService;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public IReadOnlyList<ResearchOrganism> Organismes { get; set; } = [];

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/";
        LoadOrganismes();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ReturnUrl = returnUrl;
        LoadOrganismes();

        if (Input.DateNaissance > DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
        {
            ModelState.AddModelError("Input.DateNaissance", "Vous devez avoir au moins 18 ans pour vous inscrire.");
        }

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

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, Input.Nom, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        user.DateOfBirth = Input.DateNaissance;
        user.ResearchOrganismId = Input.ResearchOrganism;

        if (Input.Avatar is not null)
        {
            var uploadsDir = Path.Combine("wwwroot", "avatars");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.Avatar.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var fs = System.IO.File.Create(filePath))
            {
                await Input.Avatar.CopyToAsync(fs);
            }

            user.Avatar = $"/avatars/{fileName}";
        }
        else if (!string.IsNullOrWhiteSpace(Input.SelectedAvatar))
        {
            var allowedAvatars = new[]
            {
                "1.png",
                "2.png",
                "3.png",
                "4.png"
            };

            if (!allowedAvatars.Contains(Input.SelectedAvatar))
            {
                ModelState.AddModelError("Input.SelectedAvatar", "Avatar invalide.");
                return Page();
            }

            user.Avatar = Input.SelectedAvatar;
        }

        var result = await _userManager.CreateAsync(user, Input.Password);
        await _userManager.AddClaimAsync(user, new Claim("Avatar", user.Avatar ?? "1.png"));
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, AppRoles.Research);

            _logger.LogInformation("Nouvel utilisateur créé : {Email}", Input.Email);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new
                {
                    userId = user.Id,
                    token = token
                },
                protocol: Request.Scheme
            );

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirme ton compte BioDomes",
                $@"
<h2>Bienvenue {Input.Nom}</h2>
<p>Clique sur le lien pour activer ton compte :</p>
<a href='{confirmationLink}'>Confirmer mon compte</a>
");

            return RedirectToPage("/Account/Login");
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }

    [BindProperty]
    public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Le nom doit contenir entre 5 et 50 caractères.")]
        [Display(Name = "Nom du chercheur")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Avatar prédéfini")]
        public string? SelectedAvatar { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [IsEmailValid]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateOnly DateNaissance { get; set; }

        [Display(Name = "Avatar")]
        public IFormFile? Avatar { get; set; }

        [Display(Name = "Organisme de recherche")]
        public string? ResearchOrganism { get; set; }
    }

    private IUserEmailStore<Infrastructures.entities.UserEntity> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<Infrastructures.entities.UserEntity>)_userStore;
    }

    private void LoadOrganismes()
    {
        Organismes = _researchOrganismService.GetAllOrganisms();
    }

    private Infrastructures.entities.UserEntity CreateUser()
    {
        try
        {
            return Activator.CreateInstance<Infrastructures.entities.UserEntity>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively override the register page.");
        }
    }
}
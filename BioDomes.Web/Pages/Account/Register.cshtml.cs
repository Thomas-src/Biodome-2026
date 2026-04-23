using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.mail;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;

public class RegisterModel : PageModel
{
   
    private readonly UserManager<Infrastructures.entities.User> _userManager;
    private readonly IUserStore<Infrastructures.entities.User> _userStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IResearchService _researchOrganismService;
    private readonly SignInManager<Infrastructures.entities.User> _signInManager;
    private readonly IUserEmailStore<Infrastructures.entities.User> _emailStore;
    private readonly IMyEmailSender _emailSender;

    public RegisterModel(
        UserManager<Infrastructures.entities.User> userManager,
        IUserStore<Infrastructures.entities.User> userStore,
        ILogger<RegisterModel> logger,
        IResearchService researchOrganismService, 
        SignInManager<Infrastructures.entities.User> signInManager,
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

        // ── Validations supplémentaires ────────────────────────────────────
        if (Input.Avatar is not null)
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowed.Contains(Input.Avatar.ContentType))
                ModelState.AddModelError("Input.Avatar",
                    "Seuls les formats JPG, PNG, GIF et WebP sont acceptés.");

            if (Input.Avatar.Length > 2 * 1024 * 1024)
                ModelState.AddModelError("Input.Avatar",
                    "L'avatar ne peut pas dépasser 2 Mo.");
        }

        if (Input.DateNaissance > DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
            ModelState.AddModelError("Input.DateNaissance",
                "Vous devez avoir au moins 18 ans pour vous inscrire.");

        if (ModelState.IsValid)
        {



            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Nom, CancellationToken.None); // Nom ≠ Email
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            user.DateOfBirth = Input.DateNaissance;
            user.ResearchOrganismId = Input.ResearchOrganism;

            if (Input.Avatar is not null)
            {
                using var ms = new MemoryStream();
                await Input.Avatar.CopyToAsync(ms);
                var uploadsDir = Path.Combine("wwwroot", "avatars");
                Directory.CreateDirectory(uploadsDir);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.Avatar.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var fs = System.IO.File.Create(filePath))
                {
                    await Input.Avatar.CopyToAsync(fs);
                }

                user.Avatar = $"/avatars/{fileName}"; // chemin relatif stocké en string
            }

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppRoles.Research);
                _logger.LogInformation("Nouvel utilisateur créé : {Email}", Input.Email);
                await _signInManager.SignInAsync(user, isPersistent: true);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError(error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }
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
        
        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [IsEmailValid]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
       
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 2 caractères.")]
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

    
    private IUserEmailStore<Infrastructures.entities.User> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<Infrastructures.entities.User>)_userStore;
    }
    
    private void LoadOrganismes()
    {
        Organismes = _researchOrganismService.GetAllOrganisms();
    }

    private Infrastructures.entities.User CreateUser()
    {
        try
        {
            return Activator.CreateInstance<Infrastructures.entities.User>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

}
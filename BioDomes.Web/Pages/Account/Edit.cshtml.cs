using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;

[Authorize]
public class EditModel : PageModel
{
    private readonly UserManager<Infrastructures.entities.User> _userManager;
    private readonly IUserStore<Infrastructures.entities.User> _userStore;
    private readonly ILogger<EditModel> _logger;
    private readonly IResearchService _researchOrganismService;
    private readonly IUserEmailStore<Infrastructures.entities.User> _emailStore;

    public EditModel(
        UserManager<Infrastructures.entities.User> userManager,
        IUserStore<Infrastructures.entities.User> userStore,
        ILogger<EditModel> logger,
        IResearchService researchOrganismService)
    {
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _emailStore = GetEmailStore();
        _researchOrganismService = researchOrganismService;
    }

    public IReadOnlyList<ResearchOrganism> Organismes { get; set; } = [];
    public string? CurrentAvatar { get; set; }
    public string? Email { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

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
            var uploadsDir = Path.Combine("wwwroot", "uploads", "avatars");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.Avatar.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var fs = System.IO.File.Create(filePath))
            {
                await Input.Avatar.CopyToAsync(fs);
            }

            if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar.StartsWith("/uploads/"))
            {
                var oldPath = Path.Combine("wwwroot", user.Avatar.TrimStart('/'));

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            user.Avatar = $"/uploads/avatars/{fileName}";
        }
        else if (!string.IsNullOrEmpty(Input.SelectedAvatar))
        {
            user.Avatar = Input.SelectedAvatar;
        }

        var result = await _userManager.UpdateAsync(user);

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

        [Display(Name = "Avatar")]
        public IFormFile? Avatar { get; set; }

        public string? SelectedAvatar { get; set; }

        [Display(Name = "Organisme de recherche")]
        public string? ResearchOrganism { get; set; }
    }

    private IUserEmailStore<Infrastructures.entities.User> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("User store must support email.");
        }
           

        return (IUserEmailStore<Infrastructures.entities.User>)_userStore;
    }

    private void LoadOrganismes()
        => Organismes = _researchOrganismService.GetAllOrganisms();
}
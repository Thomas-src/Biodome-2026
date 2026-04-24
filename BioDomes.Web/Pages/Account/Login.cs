using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Account;
public class LoginModel(SignInManager<Infrastructures.entities.User> signInManager) : PageModel
{
    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    [BindProperty]
    [Required]
    [DataType(DataType.Text)]
    [MinLength(5)]
    public string? EmailOrUsername { get; set; }

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [BindProperty]
    public bool RememberMe { get; set; }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await signInManager.UserManager.FindByEmailAsync(EmailOrUsername!) ?? await signInManager.UserManager.FindByNameAsync(EmailOrUsername!);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Identifiants incorrects.");
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(
                user.UserName,
                Password,
                RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }


            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
                
        

        ModelState.AddModelError(string.Empty, "Tentative de connexion invalide.");
        return Page();
    }
}
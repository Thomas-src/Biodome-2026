using System.Text;
using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BioDomes.Web.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<ConfirmEmailModel> _logger;

    public ConfirmEmailModel(UserManager<UserEntity> userManager, ILogger<ConfirmEmailModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public string? StatusMessage { get; set; }
    public bool IsSuccess { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string token)
    {
        _logger.LogInformation($"ConfirmEmail - UserId: {userId}, token: {token}");

        if (userId == null || token == null)
        {
            _logger.LogInformation("All null");
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            IsSuccess = false;
            StatusMessage = "Utilisateur introuvable.";
            return Page();
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        IsSuccess = result.Succeeded;
        StatusMessage = result.Succeeded
            ? "Votre adresse e-mail a été confirmée avec succès."
            : "Une erreur est survenue lors de la confirmation de votre e-mail.";

        return Page();
    }
}
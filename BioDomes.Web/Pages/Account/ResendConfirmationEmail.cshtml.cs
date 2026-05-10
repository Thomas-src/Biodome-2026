using System.ComponentModel.DataAnnotations;
using System.Text;
using BioDomes.Domains.Services.mail;
using BioDomes.Infrastructures.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BioDomes.Web.Pages.Account;

[ValidateAntiForgeryToken]
public class ResendConfirmationEmailModel : PageModel
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMyEmailSender _emailSender;
    private readonly ILogger<ResendConfirmationEmailModel> _logger;

    public ResendConfirmationEmailModel(
        UserManager<UserEntity> userManager,
        IMyEmailSender emailSender,
        ILogger<ResendConfirmationEmailModel> logger)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _logger = logger;
    }

    [BindProperty] 
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisés")]
    public string Email { get; set; } = string.Empty;

    public string? StatusMessage { get; set; }
    public bool? IsSuccess { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Email);
        if (user == null || await _userManager.IsEmailConfirmedAsync(user))
        {
            IsSuccess = true;
            StatusMessage = "Si cette adresse est associée à un compte non confirmé, un nouveau lien vient d'être envoyé.";
            return Page();
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var confirmationLink = Url.Page("/Account/ConfirmEmail", pageHandler: null,
            values: new { userId = user.Id, token = encodedToken },
            protocol: Request.Scheme
        );
        _logger.LogInformation("Renvoi du lien de confirmation à {Email}", Email);

        await _emailSender.SendEmailAsync(Email, "Confirme ton compte BioDomes",
            $@"
<h2>Confirmation de ton compte</h2>
<p>Tu as demandé un nouveau lien de confirmation. Clique ci-dessous pour activer ton compte :</p>
<a href='{confirmationLink}'>Confirmer mon compte</a>
<p>Si tu n'es pas à l'origine de cette demande, ignore cet e-mail.</p>
");
        IsSuccess = true;
        StatusMessage = "Si cette adresse est associée à un compte non confirmé, un nouveau lien vient d'être envoyé.";
        return Page();
    }
}
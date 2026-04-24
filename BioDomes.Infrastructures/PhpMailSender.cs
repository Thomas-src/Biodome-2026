using System.Net.Http.Json;
using BioDomes.Domains;
using BioDomes.Domains.Services.mail;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures;

public class PhpMailSender : IMyEmailSender
{
    private readonly HttpClient _http;
    private readonly ILogger<PhpMailSender> _logger;

    public PhpMailSender(
        HttpClient http,
        MailConfig config,
        ILogger<PhpMailSender> logger)
    {
        _http = http;
        _logger = logger;

        _http.DefaultRequestHeaders.Add("X-API-TOKEN", config.Token);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var payload = new
        {
            to = email,
            subject,
            body = htmlMessage
        };

        _logger.LogInformation("📧 Envoi email vers {Email} | Subject: {Subject}", email, subject);

        var response = await _http.PostAsJsonAsync(
            "https://panoramix.cg.helmo.be/~q240206/EVAL_V4/api.php",
            payload
        );

        var responseBody = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("✅ Email envoyé avec succès vers {Email}", email);
        }
        else
        {
            _logger.LogError(
                "❌ Échec envoi email vers {Email} | Status: {StatusCode} | Response: {Response}",
                email,
                (int)response.StatusCode,
                responseBody
            );
        }

        response.EnsureSuccessStatusCode();
    }
}
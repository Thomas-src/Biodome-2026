using System.Net.Http.Json;
using BioDomes.Domains;
using BioDomes.Domains.Services.mail;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures;

public class ResendMailSender : IMyEmailSender
{
    private readonly HttpClient _http;
    private readonly ILogger<ResendMailSender> _logger;
    private readonly string _fromAddress;

    public ResendMailSender(
        HttpClient http,
        MailConfig config,
        ILogger<ResendMailSender> logger)
    {
        _http = http;
        _logger = logger;
        _fromAddress = config.FromAddress; // ex: "BioDomes <noreply@tondomaine.com>"

        _http.BaseAddress = new Uri("https://api.resend.com/");
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ResendApiKey}");
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var payload = new
        {
            from = _fromAddress,
            to = new[] { email },
            subject,
            html = htmlMessage
        };

        _logger.LogInformation("📧 Envoi email vers {Email} | Subject: {Subject}", email, subject);

        var response = await _http.PostAsJsonAsync("emails", payload);

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
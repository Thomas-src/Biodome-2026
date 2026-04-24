namespace BioDomes.Domains.Services.mail;

public interface IMyEmailSender
{
    Task SendEmailAsync(string email, string subject, string body);
}
namespace BioDomes.Domains.Services.mail;

public interface IMyEmailSender
{
    void SendEmail(string email, string subject, string host);
}
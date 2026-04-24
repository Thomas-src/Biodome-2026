using System.Net;
using System.Net.Mail;

namespace BioDomes.Domains.Services.mail;
/**
 * Code de https://stackoverflow.com/questions/65070487/how-to-configure-email-sending-in-asp-net-core-for-identity-library
 */
public class MyEmailSender(MailConfig configuration) : IMyEmailSender
{
    public MailConfig _config { get; } = configuration;

    public void SendEmail(string email, string subject, string HtmlMessage)
    {
        using (MailMessage mm = new MailMessage(_config.Sender, email))
        {
            mm.Subject = subject;
            string body = HtmlMessage;
            mm.Body = body;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _config.SmtpHost;
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential(_config.Sender, _config.SenderPassword);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
        }
    } 
}
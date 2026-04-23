namespace BioDomes.Domains.Services.mail;

public record MailConfig(
    string Sender,
    string SenderPassword,
    string SmtpHost
);
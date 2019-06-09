namespace SmartSchedule.Infrastucture.Email
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using SmartSchedule.Application.Interfaces;

    public class EmailService : IEmailService
    {
        public virtual async Task SendEmail(string email, string subject, string message)
        {
            using (var client = new SmtpClient(EmailConfiguration.SmtpClient))
            {
                var credential = new NetworkCredential
                {
                    UserName = EmailConfiguration.EmailAddress,
                    Password = EmailConfiguration.Password
                };

                client.Credentials = credential;
                client.Host = EmailConfiguration.Host;
                client.Port = EmailConfiguration.Port;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress(EmailConfiguration.EmailAddress);
                    emailMessage.Subject = subject;
                    emailMessage.Body = WebUtility.HtmlDecode(message);
                    emailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(emailMessage);
                }
            }
        }
    }
}

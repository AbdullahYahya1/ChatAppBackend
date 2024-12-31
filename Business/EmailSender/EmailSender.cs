using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Business.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _Configuration;

        public EmailSender(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

                var emailSettings = _Configuration.GetSection("EmailSettings");
                var mail = emailSettings["Username"];
                var pass = emailSettings["Password"];
                var host = emailSettings["Host"];
                var port = int.Parse(emailSettings["Port"]);
                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(mail, pass),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);
                await client.SendMailAsync(mailMessage);
        }
    }
}


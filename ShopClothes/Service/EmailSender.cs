using System.Net.Mail;
using System.Net;
using ShopClothes.WebApp.Models.AccountViewModel;
using Microsoft.Extensions.Options;
using Azure.Core;

namespace ShopClothes.WebApp.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings )
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string messageHtml)
        {

            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = _emailSettings.UserName,
                    Password = _emailSettings.Password
                };

                client.Credentials = credentials;
                client.Host = _emailSettings.Host;
                client.Port = _emailSettings.Port;
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.UserName),
                    Subject = subject,
                    Body = messageHtml,
                    IsBodyHtml = true
                };

                message.To.Add(email);

                await client.SendMailAsync(message);
            }
        }
    }
}

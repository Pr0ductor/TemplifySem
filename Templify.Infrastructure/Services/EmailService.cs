using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Templify.Application.Interfaces.Services;
using Templify.Persistence.Settings;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Templify.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            try
            {
                await SendMessage(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when sending a letter: {ex.Message}");
                throw;
            }
        }

        public async Task SendMessage(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = _emailSettings.CheckCertificateRevocation;
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }
    }

}

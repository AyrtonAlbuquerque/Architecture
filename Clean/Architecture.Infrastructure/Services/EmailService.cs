using System.Net.Mail;
using Architecture.Application.Abstractions.Services;
using Architecture.Application.Common;

namespace Architecture.Infrastructure.Services
{
    public class EmailService(Settings settings, SmtpClient client) : IEmailService
    {
        public async Task SendAsync(string address, string subject, string body)
        {
            await client.SendMailAsync(new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(settings.Mail.Email, settings.Mail.Name),
                To = { new MailAddress(address) },
                Subject = subject,
                Body = body
            });

            client.Dispose();
        }
    }
}
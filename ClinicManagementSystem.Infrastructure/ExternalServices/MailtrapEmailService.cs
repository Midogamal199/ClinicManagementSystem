using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class MailtrapEmailService : IEmailService
    {
        private readonly MailtrapOptions _options;

        public MailtrapEmailService(IOptions<MailtrapOptions> options)
        {
            _options = options.Value;
        }

        public Task SendEmailAsync(string toEmail, string subject, string body)
        {
            return SendEmailAsync(toEmail, subject, body, new List<EmailAttachment>());
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            IEnumerable<EmailAttachment> attachments)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder { TextBody = body };

            foreach (var attachment in attachments)
            {
                builder.Attachments.Add(
                    attachment.FileName,
                    attachment.Content,
                    ContentType.Parse(attachment.ContentType));
            }

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.Username, _options.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
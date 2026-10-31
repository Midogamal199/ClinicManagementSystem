using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/octet-stream";
        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
        public interface IEmailService
        {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailAsync(
                    string toEmail,
                    string subject,
                    string body,
                    IEnumerable<EmailAttachment> attachments);
        }
}
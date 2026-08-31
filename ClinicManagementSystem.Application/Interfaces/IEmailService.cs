using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
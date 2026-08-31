using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateAndStoreOtpAsync(string email);
        bool ValidateOtp(string email, string code);
    }
}
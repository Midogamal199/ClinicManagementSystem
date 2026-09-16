using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> UserExistsAsync(string email);
        Task<bool> PatientHasAccountAsync(Guid patientId);
        Task<bool> EmployeeHasAccountAsync(Guid employeeId);
        Task<AppIdentityResult> RegisterPatientAsync(string email, string password, Guid patientId);
        Task<AppIdentityResult> CreateStaffAccountAsync(string email, string password, string role, Guid employeeId);
        Task<AppIdentityResult> LoginAsync(string email, string password);
        Task<AppIdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<AppIdentityResult> GetUserInfoAsync(Guid userId);
        Task StoreRefreshTokenAsync(Guid userId, string token, DateTime expiresAt);
        Task<Guid?> ValidateAndConsumeRefreshTokenAsync(string token);
        Task RevokeAllUserTokensAsync(Guid userId);
        Task<Guid?> GetUserIdByEmailAsync(string email);
        Task<AppIdentityResult> ResetPasswordAsync(string email, string newPassword);


    }
}
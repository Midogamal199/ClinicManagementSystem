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
    }
}
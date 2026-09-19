using System.Security.Claims;
using ClinicManagementSystem.Application.Interfaces;

namespace ClinicManagementSystem.WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal? _user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext?.User;
        }
        public Guid? UserId
        {
            get
            {
                var value = _user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(value, out var id) ? id : null;



            }
        }

        public Guid? PatientId
        {
            get
            {
                var value = _user?.FindFirst("PatientId")?.Value;
                return Guid.TryParse(value, out var id) ? id : null;
            }

        
        }

        public Guid? EmployeeId
        {
            get
            {
                var value = _user?.FindFirst("EmployeeId")?.Value;
                return Guid.TryParse(value, out var id) ? id : null;
            }
        }

        public IReadOnlyList<string> Roles =>
            _user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();

        public bool IsInRole(string role) => _user?.IsInRole(role) ?? false;

    }
}

using System.Collections.Generic;

namespace ClinicManagementSystem.Application.Common.Models
{
    public class AppIdentityResult
    {
        public bool Succeeded { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public Guid? PatientId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
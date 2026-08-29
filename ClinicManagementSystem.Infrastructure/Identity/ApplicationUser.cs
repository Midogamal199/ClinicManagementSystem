using ClinicManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public Guid? PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Doctor: BaseEntity
    {
        public string LicenseNumber { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}

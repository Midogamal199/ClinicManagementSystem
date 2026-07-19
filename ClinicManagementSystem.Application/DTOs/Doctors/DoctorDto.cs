using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Doctors
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string LicenseNumber { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public List<string> Specialties { get; set; } = new();
    }
}

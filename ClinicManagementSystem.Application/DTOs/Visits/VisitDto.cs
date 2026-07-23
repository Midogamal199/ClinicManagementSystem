using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Visits
{
   public class VisitDto
    {
        public Guid Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string Notes { get; set; }
        public Guid AppointmentId { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorFullName { get; set; }
        public string DoctorLicenseNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Appointments
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; }
        public Guid PatientId { get; set; }
        public string PatientFullName { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorFullName { get; set; }

    }
}

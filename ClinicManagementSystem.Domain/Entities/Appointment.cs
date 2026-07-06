using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Appointment: BaseEntity
    {
        public DateTime ScheduledAt { get; set; }
        public AppointmentStatus Status { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Visit? Visit { get; set; }
    }
}

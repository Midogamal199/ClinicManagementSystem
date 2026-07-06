using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Visit: BaseEntity
    {
        public DateTime VisitDate { get; set; }
        public string Notes { get; set; }

        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<LabTest> LabTests { get; set; } = new List<LabTest>();

    }
}

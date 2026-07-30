using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Prescriptions
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public DateTime IssuedAt { get; set; }
        public Guid VisitId { get; set; }
        public string PatientFullName { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();
    }
}


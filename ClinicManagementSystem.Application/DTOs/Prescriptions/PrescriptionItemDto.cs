using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.DTOs.Prescriptions
{
    public class PrescriptionItemDto
    {
        public Guid Id { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
    }
}

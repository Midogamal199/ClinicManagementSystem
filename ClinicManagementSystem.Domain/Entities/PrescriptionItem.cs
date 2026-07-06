using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
   public class PrescriptionItem: BaseEntity
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public Guid PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
    }
}

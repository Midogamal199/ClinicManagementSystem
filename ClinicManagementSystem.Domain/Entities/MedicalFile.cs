using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class MedicalFile:BaseEntity
    {
        public DateTime LastUpdated { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}

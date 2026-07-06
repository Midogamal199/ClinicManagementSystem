using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
     public class Prescription: BaseEntity
    {
        public DateTime IssuedAt { get; set; }
        public Guid VisitId { get; set; }
        public Visit Visit { get; set; }
        public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();


    }
}

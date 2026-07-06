using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Diagnosis: BaseEntity
    {
        public string Description { get; set; }
        public string IcdCode { get; set; }

        public Guid VisitId { get; set; }
        public Visit Visit { get; set; }
    }
}

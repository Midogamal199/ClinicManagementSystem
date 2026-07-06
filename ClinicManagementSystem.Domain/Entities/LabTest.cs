using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
     public class LabTest:BaseEntity
    {
        public string TestType { get; set; }
        public LabTestStatus Status { get; set; }
        public string? ResultFileUrl { get; set; }

        public Guid VisitId { get; set; }
        public Visit Visit { get; set; }
    }
}

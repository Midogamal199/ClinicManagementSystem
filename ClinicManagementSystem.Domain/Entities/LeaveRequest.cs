using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
   public class LeaveRequest:BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}

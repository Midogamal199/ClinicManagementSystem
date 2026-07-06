using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Attendance:BaseEntity
    {
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}

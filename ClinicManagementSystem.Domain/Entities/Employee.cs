using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Employee:BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public int AnnualLeaveBalance { get; set; } = 21;

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    }
}

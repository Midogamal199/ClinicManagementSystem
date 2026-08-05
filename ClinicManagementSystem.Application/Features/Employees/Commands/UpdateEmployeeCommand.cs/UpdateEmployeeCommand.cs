using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Commands.UpdateEmployeeCommand.cs
{
    public class UpdateEmployeeCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public Guid DepartmentId { get; set; }
    
    }
}

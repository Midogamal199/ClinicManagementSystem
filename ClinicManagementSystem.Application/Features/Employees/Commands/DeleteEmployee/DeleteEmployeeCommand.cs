using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInCommand:IRequest<Guid>
    {
        public Guid EmployeeId { get; set; }
    }
}

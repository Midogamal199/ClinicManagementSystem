using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommand: IRequest<Guid>
    {
        public string LicenseNumber { get; set; }
        public Guid EmployeeId { get; set; }
        public List<Guid> SpecialtyIds { get; set; } = new();
    }
}

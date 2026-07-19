using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.UpdateDoctor
{
    public class UpdateDoctorCommand:IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string LicenseNumber { get; set; }
        public List<Guid> SpecialtyIds { get; set; } = new();
    }
}

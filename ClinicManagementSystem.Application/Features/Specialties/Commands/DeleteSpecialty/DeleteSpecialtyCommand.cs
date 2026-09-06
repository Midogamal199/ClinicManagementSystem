using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.DeleteSpecialty
{
    public class DeleteSpecialtyCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}

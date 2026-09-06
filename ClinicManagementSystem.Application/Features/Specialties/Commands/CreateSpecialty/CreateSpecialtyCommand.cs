using System;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.CreateSpecialty
{
    public class CreateSpecialtyCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}
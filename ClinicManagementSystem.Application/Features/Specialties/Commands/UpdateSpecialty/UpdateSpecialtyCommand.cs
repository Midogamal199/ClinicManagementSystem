using System;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.UpdateSpecialty
{
    public class UpdateSpecialtyCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
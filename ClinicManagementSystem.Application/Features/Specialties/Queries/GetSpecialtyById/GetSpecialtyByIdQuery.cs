using System;
using ClinicManagementSystem.Application.DTOs.Specialties;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Queries.GetSpecialtyById
{
    public class GetSpecialtyByIdQuery : IRequest<SpecialtyDto>
    {
        public Guid Id { get; set; }
    }
}
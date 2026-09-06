using System.Collections.Generic;
using ClinicManagementSystem.Application.DTOs.Specialties;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Queries.GetAllSpecialties
{
    public class GetAllSpecialtiesQuery : IRequest<List<SpecialtyDto>>
    {
    }
}
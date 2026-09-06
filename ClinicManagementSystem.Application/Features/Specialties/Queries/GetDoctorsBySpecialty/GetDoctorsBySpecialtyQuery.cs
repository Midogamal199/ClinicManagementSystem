using System;
using System.Collections.Generic;
using ClinicManagementSystem.Application.DTOs.Specialties;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Queries.GetDoctorsBySpecialty
{
    public class GetDoctorsBySpecialtyQuery : IRequest<List<DoctorSummaryDto>>
    {
        public Guid SpecialtyId { get; set; }
    }
}
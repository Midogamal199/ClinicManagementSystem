using System.Collections.Generic;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetTopVisitedDoctors
{
    public class GetTopVisitedDoctorsQuery : IRequest<List<TopVisitedDoctorDto>>
    {
    }
}
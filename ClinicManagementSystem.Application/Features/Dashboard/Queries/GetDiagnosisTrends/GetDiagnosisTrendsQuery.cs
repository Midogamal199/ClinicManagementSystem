using System.Collections.Generic;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDiagnosisTrends
{
    public class GetDiagnosisTrendsQuery : IRequest<List<DiagnosisTrendItemDto>>
    {
    }
}
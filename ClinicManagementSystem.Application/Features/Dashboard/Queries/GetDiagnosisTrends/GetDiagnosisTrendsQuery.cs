using System.Collections.Generic;
using ClinicManagementSystem.Application.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDiagnosisTrends
{
    public class GetDiagnosisTrendsQuery : IRequest<List<DiagnosisTrendItemDto>>
    {
    }
}
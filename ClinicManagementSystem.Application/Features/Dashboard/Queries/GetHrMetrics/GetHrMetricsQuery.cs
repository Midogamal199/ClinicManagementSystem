using ClinicManagementSystem.Domain.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetHrMetrics
{
    public class GetHrMetricsQuery : IRequest<HrMetricsDto>
    {
    }
}
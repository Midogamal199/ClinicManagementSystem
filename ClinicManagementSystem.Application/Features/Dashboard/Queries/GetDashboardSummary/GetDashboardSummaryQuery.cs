using ClinicManagementSystem.Domain.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDashboardSummary
{
    public class GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>
    {
    }
}
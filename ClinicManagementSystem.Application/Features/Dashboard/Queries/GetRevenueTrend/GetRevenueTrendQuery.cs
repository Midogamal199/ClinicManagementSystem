using System.Collections.Generic;
using ClinicManagementSystem.Application.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetRevenueTrend
{
    public class GetRevenueTrendQuery : IRequest<List<RevenueTrendItemDto>>
    {
    }
}
using System.Collections.Generic;
using ClinicManagementSystem.Application.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetSpecialtyDistribution
{
    public class GetSpecialtyDistributionQuery : IRequest<List<SpecialtyDistributionItemDto>>
    {
    }
}
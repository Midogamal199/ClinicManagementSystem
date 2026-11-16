using System.Collections.Generic;
using ClinicManagementSystem.Application.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetGenderDistribution
{
    public class GetGenderDistributionQuery : IRequest<List<GenderDistributionItemDto>>
    {
    }
}
using System.Collections.Generic;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetGenderDistribution
{
    public class GetGenderDistributionQuery : IRequest<List<GenderDistributionItemDto>>
    {
    }
}
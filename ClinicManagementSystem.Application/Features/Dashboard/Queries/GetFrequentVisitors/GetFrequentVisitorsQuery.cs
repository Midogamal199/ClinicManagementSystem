using System.Collections.Generic;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetFrequentVisitors
{
    public class GetFrequentVisitorsQuery : IRequest<List<FrequentVisitorDto>>
    {
    }
}
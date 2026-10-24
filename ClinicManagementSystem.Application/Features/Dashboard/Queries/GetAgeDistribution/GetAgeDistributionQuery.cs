using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Dashboard;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetAgeDistribution
{
    public class GetAgeDistributionQuery: IRequest<List<AgeDistributionItemDto>>
    {

    }
}

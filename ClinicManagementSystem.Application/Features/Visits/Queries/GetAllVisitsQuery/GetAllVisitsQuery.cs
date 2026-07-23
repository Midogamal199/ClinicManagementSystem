using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Visits;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Visits.Queries.GetAllVisitsQuery
{
    public class GetAllVisitsQuery : IRequest<PaginatedResult<VisitDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

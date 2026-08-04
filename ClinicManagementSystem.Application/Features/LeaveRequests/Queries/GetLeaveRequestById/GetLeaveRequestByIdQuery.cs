using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.LeaveRequests;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById
{
    public class GetLeaveRequestByIdQuery: IRequest<LeaveRequestDto>
    {
        public Guid Id { get; set; }

        public GetLeaveRequestByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

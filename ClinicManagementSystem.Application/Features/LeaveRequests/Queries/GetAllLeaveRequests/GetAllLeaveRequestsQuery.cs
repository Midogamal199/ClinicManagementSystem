using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.LeaveRequests;
using ClinicManagementSystem.Domain.Enums;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests
{
    public class GetAllLeaveRequestsQuery : IRequest<PaginatedResult<LeaveRequestDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? EmployeeId { get; set; }
        public LeaveStatus? Status { get; set; }
    }
}
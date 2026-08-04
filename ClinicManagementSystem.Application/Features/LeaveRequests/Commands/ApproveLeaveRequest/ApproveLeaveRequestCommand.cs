using MediatR;

namespace ClinicManagementSystem.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest
{
    public class ApproveLeaveRequestCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public ApproveLeaveRequestCommand(Guid id)
        {
            Id = id;
        }
    }
}
using MediatR;

namespace ClinicManagementSystem.Application.Features.LeaveRequests.Commands.RejectLeaveRequest
{
    public class RejectLeaveRequestCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public RejectLeaveRequestCommand(Guid id)
        {
            Id = id;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest
{
    public class ApproveLeaveRequestCommandHandler : IRequestHandler<ApproveLeaveRequestCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveLeaveRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(ApproveLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.Repository<LeaveRequest>().GetByIdAsync(request.Id);

            if (leaveRequest is null)
            {
                throw new KeyNotFoundException($"Leave request with Id '{request.Id}' was not found.");
            }
            if (leaveRequest.Status != LeaveStatus.Pending)
            {
                throw new InvalidOperationException(
                                   $"Cannot approve a leave request with status '{leaveRequest.Status}'. Only pending requests can be approved.");
            }
            leaveRequest.Status = LeaveStatus.Approved;
            _unitOfWork.Repository<LeaveRequest>().Update(leaveRequest);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;

        }
    }
}

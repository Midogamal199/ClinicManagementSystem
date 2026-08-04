using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(request.EmployeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' was not found.");
            }
            var hasOverlap = await _unitOfWork.LeaveRequestRepository.HasOverlapAsync(request.EmployeeId, request.StartDate, request.EndDate);
            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "This employee already has a leave request overlapping these dates.");
            }
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = request.EmployeeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = LeaveStatus.Pending
            };
            await _unitOfWork.Repository<LeaveRequest>().AddAsync(leaveRequest);
            await _unitOfWork.SaveChangesAsync();

            return leaveRequest.Id;
        }
    }
}

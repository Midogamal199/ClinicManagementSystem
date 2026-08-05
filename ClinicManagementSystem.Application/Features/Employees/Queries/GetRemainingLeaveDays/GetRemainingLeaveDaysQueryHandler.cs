using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Queries.GetRemainingLeaveDays
{
    public class GetRemainingLeaveDaysQueryHandler : IRequestHandler<GetRemainingLeaveDaysQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRemainingLeaveDaysQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(GetRemainingLeaveDaysQuery request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(request.EmployeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' was not found.");
            }
            var approvedLeaves = await _unitOfWork.Repository<LeaveRequest>().FindAsync(
                l => l.EmployeeId == request.EmployeeId && l.Status == LeaveStatus.Approved);
            var usedDays = approvedLeaves.Sum(l => (l.EndDate - l.StartDate).Days + 1);

            return employee.AnnualLeaveBalance - usedDays;

        }
    }
}

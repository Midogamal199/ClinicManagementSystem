using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Commands.CheckOut
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckOutCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(request.EmployeeId);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' was not found.");
            }
            var openAttendance = await _unitOfWork.AttendanceRepository
                .GetOpenAttendanceForEmployeeAsync(request.EmployeeId);

            if (openAttendance is null)
            {
                throw new InvalidOperationException(
                    "This employee has no open check-in to check out from.");
            }
            openAttendance.CheckOut = DateTime.UtcNow;

            _unitOfWork.Repository<Attendance>().Update(openAttendance);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckInCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(request.EmployeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' was not found.");
            }
            var openAttendance = await _unitOfWork.AttendanceRepository.GetOpenAttendanceForEmployeeAsync(request.EmployeeId);
            if (openAttendance is not null)
            {
                throw new InvalidOperationException($"Employee with Id '{request.EmployeeId}' already has an open attendance record.");
            }
            var attendance = new Attendance
            {
                EmployeeId = request.EmployeeId,
                CheckIn = DateTime.UtcNow
            };
            await _unitOfWork.Repository<Attendance>().AddAsync(attendance);
            await _unitOfWork.SaveChangesAsync();

            return attendance.Id;
        }
    }
}

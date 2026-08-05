using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Queries.GetWorkingHours
{
    public class GetWorkingHoursQueryHandler : IRequestHandler<GetWorkingHoursQuery, double>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkingHoursQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<double> Handle(GetWorkingHoursQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(request.AttendanceId);
            if (attendance is null)
            {
                throw new KeyNotFoundException($"Attendance with Id '{request.AttendanceId}' was not found.");
            }

            if (attendance.CheckOut is null)
            {
                throw new InvalidOperationException(
                    "Cannot calculate working hours for an attendance record that has no check-out yet.");
            }
            var duration = attendance.CheckOut.Value - attendance.CheckIn;
            return Math.Round(duration.TotalHours, 2);

        }
    }
}

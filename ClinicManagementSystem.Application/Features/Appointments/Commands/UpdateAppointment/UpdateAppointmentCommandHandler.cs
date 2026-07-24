using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAppointmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(request.Id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with Id '{request.Id}' was not found.");
            }
            var hasConflict = await _unitOfWork.AppointmentRepository.HasConflictAsync(appointment.DoctorId, request.ScheduledAt, request.Id);
            if (hasConflict)
            {
                throw new InvalidOperationException(
                    "The selected doctor already has another appointment at this time.");
            }
            appointment.ScheduledAt = request.ScheduledAt;
            appointment.Status = request.Status;
            _unitOfWork.Repository<Appointment>().Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;

        }
    }
}

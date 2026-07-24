using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(request.PatientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(request.DoctorId);
            if (doctor is null)
            {
                throw new KeyNotFoundException($"Doctor with Id '{request.DoctorId}' was not found.");
            }
            var hasConflict = await _unitOfWork.AppointmentRepository.HasConflictAsync(request.DoctorId, request.ScheduledAt);
            if (hasConflict)
            {
                throw new InvalidOperationException(
                    "The selected doctor already has an appointment at this time.");
            }
            var appointment = new Appointment
            {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
                ScheduledAt = request.ScheduledAt,
                Status =AppointmentStatus.Scheduled
            };
            await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return appointment.Id;
        }
    }
}

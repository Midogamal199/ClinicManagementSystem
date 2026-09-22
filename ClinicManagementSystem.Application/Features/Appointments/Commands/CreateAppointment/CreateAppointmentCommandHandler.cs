using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var targetPatientId =request.PatientId;
            if (_currentUserService.IsInRole("Patient")) 
            {
                if (!_currentUserService.PatientId.HasValue)
                {
                    throw new ForbiddenAccessException("No patient profile linked to this user.");
                }
                if (request.PatientId != Guid.Empty && request.PatientId != _currentUserService.PatientId.Value)
                {
                    throw new ForbiddenAccessException("You cannot book an appointment for another patient.");
                }

                targetPatientId = _currentUserService.PatientId.Value;
            
                }
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(targetPatientId);
            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
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
                PatientId = targetPatientId,
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

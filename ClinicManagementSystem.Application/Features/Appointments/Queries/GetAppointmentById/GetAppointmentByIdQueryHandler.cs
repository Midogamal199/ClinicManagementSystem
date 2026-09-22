using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.Appointments;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAppointmentByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdWithDetailsAsync(request.Id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with Id '{request.Id}' was not found.");
            }
            if (_currentUserService.IsInRole("Patient") && appointment.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to view an appointment that does not belong to you.");
            }
            if (_currentUserService.IsInRole("Doctor") && appointment.DoctorId != _currentUserService.DoctorId)
            {
                throw new ForbiddenAccessException("You are not allowed to view an appointment assigned to another doctor.");
            }
            return _mapper.Map<AppointmentDto>(appointment);

        }
    }
}

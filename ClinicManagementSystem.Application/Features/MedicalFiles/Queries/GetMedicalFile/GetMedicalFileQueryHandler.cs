using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.MedicalFiles;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.MedicalFiles.Queries.GetMedicalFile
{
    public class GetMedicalFileQueryHandler : IRequestHandler<GetMedicalFileQuery, MedicalFileDto>
    {
        private readonly IMedicalFileRepository _medicalFileRepository;
        private readonly ICurrentUserService _currentUserService;
        public GetMedicalFileQueryHandler(
           IMedicalFileRepository medicalFileRepository,
           ICurrentUserService currentUserService)
        {
            _medicalFileRepository = medicalFileRepository;
            _currentUserService = currentUserService;
        }

        public async Task<MedicalFileDto> Handle(GetMedicalFileQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.IsInRole("Patient") && request.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to view a medical file that does not belong to you.");

            }
            if (_currentUserService.IsInRole("Doctor"))
            {
                var doctorId = _currentUserService.DoctorId;
                var hasAppointment = doctorId is not not null &&
                    await _medicalFileRepository.DoctorHasAppointmentWithPatientAsync(doctorId.Value, request.PatientId);
                if (!hasAppointment)
                {
                    throw new ForbiddenAccessException("You are not allowed to view the medical file of a patient you have no appointment with.");
                }
            }
            var medicalFile=await _medicalFileRepository.GetByPatientIdAsync(request.PatientId);
            if (medicalFile is null)
            {
                throw new KeyNotFoundException($"Patient with Id '{request.PatientId}' was not found.");
            }
            return medicalFile;
        }
    }
}

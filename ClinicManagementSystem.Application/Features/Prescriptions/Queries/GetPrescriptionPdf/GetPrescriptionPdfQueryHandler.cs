using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionPdf
{
    public class GetPrescriptionPdfQueryHandler : IRequestHandler<GetPrescriptionPdfQuery, PrescriptionPdfResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPrescriptionPdfGenerator _pdfGenerator;
        public GetPrescriptionPdfQueryHandler(
          IUnitOfWork unitOfWork,
          ICurrentUserService currentUserService,
          IPrescriptionPdfGenerator pdfGenerator)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _pdfGenerator = pdfGenerator;
        }
        public async Task<PrescriptionPdfResult> Handle(GetPrescriptionPdfQuery request, CancellationToken cancellationToken)
        {
            var prescription = await _unitOfWork.PrescriptionRepository.GetByIdForPdfAsync(request.Id);
            if (prescription is null)
            {
                throw new KeyNotFoundException($"Prescription with Id '{request.Id}' was not found.");
            }
            var appointment = prescription.Visit.Appointment;
            if(_currentUserService.IsInRole("patient") && appointment.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to print a prescription that does not belong to you.");
            }
            if (_currentUserService.IsInRole("Doctor") && appointment.DoctorId != _currentUserService.DoctorId)
            {
                throw new ForbiddenAccessException("You are not allowed to print a prescription issued by another doctor.");
            }
            var model = new PrescriptionPdfModel
            {
                PrescriptionId=prescription.Id, 
                IssuedAt=prescription.IssuedAt,
                PatientFullName=appointment.Patient.FullName,
                DoctorFullName = appointment.Doctor.Employee.FullName,
                DoctorLicenseNumber = appointment.Doctor.LicenseNumber,
                Items=prescription.Items.Select(i=>new PrescriptionItemDto
                {
                    Id = i.Id,
                    MedicineName = i.MedicineName,
                    Dosage = i.Dosage,
                    DurationInDays = i.DurationInDays

                }).ToList()
            };
            return new PrescriptionPdfResult
            {
                Content=_pdfGenerator.Generate(model),
                FileName = $"prescription-{prescription.IssuedAt:yyyyMMdd}-{prescription.Id.ToString()[..8]}.pdf"

            };
        }
    }
}

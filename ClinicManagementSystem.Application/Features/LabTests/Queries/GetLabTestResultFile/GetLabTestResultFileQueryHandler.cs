using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.LabTests;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LabTests.Queries.GetLabTestResultFile
{
    public class GetLabTestResultFileQueryHandler : IRequestHandler<GetLabTestResultFileQuery, LabTestFileResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        public GetLabTestResultFileQueryHandler(
          IUnitOfWork unitOfWork,
          ICurrentUserService currentUserService,
          IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
        }

        public async Task<LabTestFileResult> Handle(GetLabTestResultFileQuery request, CancellationToken cancellationToken)
        {
            var labTest = await _unitOfWork.LabTestRepository.GetByIdWithDetailsAsync(request.Id);
            if (labTest is null)
            {
                throw new KeyNotFoundException($"Lab test with Id '{request.Id}' was not found.");
            }
            var appointment = labTest.Visit.Appointment;
            if (_currentUserService.IsInRole("Patient") && appointment.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to download a result that does not belong to you.");
            }

            if (_currentUserService.IsInRole("Doctor") && appointment.DoctorId != _currentUserService.DoctorId)
            {
                throw new ForbiddenAccessException("You are not allowed to download a result for another doctor's patient.");
            }
            if (string.IsNullOrWhiteSpace(labTest.ResultFileUrl))
            {
                throw new KeyNotFoundException("No result file has been uploaded for this lab test yet.");
            }

            var stream = await _fileStorageService.OpenReadAsync(labTest.ResultFileUrl);
            if (stream is null)
            {
                throw new KeyNotFoundException("The result file could not be found on the server.");
            }
            return new LabTestFileResult
            {
                Content = stream,
                ContentType = GetContentType(labTest.ResultFileUrl),
                FileName = labTest.ResultFileName ?? Path.GetFileName(labTest.ResultFileUrl)
            };

        }

        private static string GetContentType(string path)
        {
            return Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}

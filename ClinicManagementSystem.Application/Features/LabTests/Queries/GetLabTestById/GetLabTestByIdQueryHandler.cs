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

namespace ClinicManagementSystem.Application.Features.LabTests.Queries.GetLabTestById
{
    public class GetLabTestByIdQueryHandler : IRequestHandler<GetLabTestByIdQuery, LabTestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetLabTestByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<LabTestDto> Handle(GetLabTestByIdQuery request, CancellationToken cancellationToken)
        {
            var labTest = await _unitOfWork.LabTestRepository.GetByIdWithDetailsAsync(request.Id);
            if (labTest is null)
            {
                throw new KeyNotFoundException($"Lab test with Id '{request.Id}' was not found.");
            }
            var appointment = labTest.Visit.Appointment;
            if (_currentUserService.IsInRole("Patient") && appointment.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to view a lab test that does not belong to you.");
            }
            if (_currentUserService.IsInRole("Doctor") && appointment.DoctorId != _currentUserService.DoctorId)
            {
                throw new ForbiddenAccessException("You are not allowed to view a lab test for another doctor's patient.");
            }
            return new LabTestDto
            {
                Id = labTest.Id,
                TestType = labTest.TestType,
                Status = labTest.Status.ToString(),
                ResultFileUrl = labTest.ResultFileUrl,
                ResultFileName = labTest.ResultFileName,
                UploadedAt = labTest.UploadedAt,
                VisitId = labTest.VisitId
            };
        }
    }
}

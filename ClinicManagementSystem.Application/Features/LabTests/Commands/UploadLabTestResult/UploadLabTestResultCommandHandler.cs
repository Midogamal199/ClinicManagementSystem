using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LabTests.Commands.UploadLabTestResult
{
    public class UploadLabTestResultCommandHandler: IRequestHandler<UploadLabTestResultCommand, Unit>

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public UploadLabTestResultCommandHandler(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<Unit> Handle(UploadLabTestResultCommand request, CancellationToken cancellationToken)
        {
            var labTest = await _unitOfWork.Repository<LabTest>().GetByIdAsync(request.LabTestId);
            if (labTest is null)
            {
                throw new KeyNotFoundException($"Lab test with Id '{request.LabTestId}' was not found.");
            }
            var uploadResult = await _fileStorageService.SaveFileAsync(request.ResultFile, "labtests");
            labTest.ResultFileUrl = uploadResult.FileUrl;
            labTest.ResultFileName = uploadResult.FileName;
            labTest.UploadedAt = DateTime.UtcNow;
            labTest.Status = LabTestStatus.Completed;
            _unitOfWork.Repository<LabTest>().Update(labTest);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;


        }
    }
}

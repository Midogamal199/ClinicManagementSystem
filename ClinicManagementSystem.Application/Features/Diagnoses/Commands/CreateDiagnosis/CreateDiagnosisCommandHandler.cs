using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Commands.CreateDiagnosis
{
    public class CreateDiagnosisCommandHandler : IRequestHandler<CreateDiagnosisCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDiagnosisCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(request.VisitId);
            if (visit is null)
            {
                throw new KeyNotFoundException($"Visit with Id '{request.VisitId}' was not found.");
            }

            var diagnosis = new Diagnosis
            {
                VisitId = request.VisitId,
                Description = request.Description,
                IcdCode = request.IcdCode
            }; 
            await _unitOfWork.Repository<Diagnosis>().AddAsync(diagnosis);
            await _unitOfWork.SaveChangesAsync();

            return diagnosis.Id;
        }
    }
}

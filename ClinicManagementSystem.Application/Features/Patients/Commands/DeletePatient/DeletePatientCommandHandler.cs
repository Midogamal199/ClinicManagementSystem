using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Commands.DeletePatient
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;   
        public DeletePatientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(request.Id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Patient with Id '{request.Id}' was not found.");
            }
            _unitOfWork.Repository<Patient>().Delete(patient);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

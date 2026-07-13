using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePatientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(request.Id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Patient with Id '{request.Id}' was not found.");
            }
            patient.FullName = request.FullName;
            patient.DateOfBirth = request.DateOfBirth;
            patient.Gender = request.Gender;
            patient.PhoneNumber = request.PhoneNumber;
            patient.Address = request.Address;
            _unitOfWork.Repository<Patient>().Update(patient);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;



        }
    }
}

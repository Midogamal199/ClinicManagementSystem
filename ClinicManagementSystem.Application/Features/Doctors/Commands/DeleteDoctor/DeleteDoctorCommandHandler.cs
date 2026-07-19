using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.DeleteDoctor
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDoctorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(request.Id);
            if (doctor is null)
            {
                throw new KeyNotFoundException($"Doctor with Id '{request.Id}' was not found.");
            }
            _unitOfWork.Repository<Doctor>().Delete(doctor);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

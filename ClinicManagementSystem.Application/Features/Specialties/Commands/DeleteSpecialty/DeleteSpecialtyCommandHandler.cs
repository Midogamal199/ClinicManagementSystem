using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.DeleteSpecialty
{
    public class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpecialtyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var specialty = await _unitOfWork.Repository<Specialty>().GetByIdAsync(request.Id);
            if (specialty is null)
            {
                throw new KeyNotFoundException($"Specialty with Id '{request.Id}' was not found.");
            }
            var hasLinkedDoctors = await _unitOfWork.SpecialtyRepository.HasLinkedDoctorsAsync(request.Id);
            if (hasLinkedDoctors)
            {
                throw new InvalidOperationException(
                    "Cannot delete this specialty because one or more doctors are currently linked to it.");
            }
            _unitOfWork.Repository<Specialty>().Delete(specialty);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

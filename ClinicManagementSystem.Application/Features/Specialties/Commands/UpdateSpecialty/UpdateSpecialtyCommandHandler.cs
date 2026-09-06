using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.UpdateSpecialty
{
    public class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpdateSpecialtyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var specialty = await _unitOfWork.Repository<Specialty>().GetByIdAsync(request.Id);
            if (specialty is null)
            {
                throw new KeyNotFoundException($"Specialty with Id '{request.Id}' was not found.");
            }
            var normalizedName = request.Name.Trim().ToLower();
            var duplicates = await _unitOfWork.Repository<Specialty>().FindAsync(
                s => s.Name.Trim().ToLower() == normalizedName && s.Id != request.Id);

            if (duplicates.Any())
            {
                throw new InvalidOperationException($"A specialty named '{request.Name}' already exists.");
            }

            specialty.Name = request.Name;
            _unitOfWork.Repository<Specialty>().Update(specialty);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;


        }
    }
}

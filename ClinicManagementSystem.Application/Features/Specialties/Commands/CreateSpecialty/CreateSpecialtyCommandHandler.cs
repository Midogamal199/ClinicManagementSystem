using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.CreateSpecialty
{
    public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSpecialtyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var normalizedName = request.Name.Trim().ToLower();
            var existing = await _unitOfWork.Repository<Specialty>().FindAsync(
               s => s.Name.Trim().ToLower() == normalizedName);
            if (existing.Any())
            {
                throw new InvalidOperationException($"A specialty named '{request.Name}' already exists.");
            }
            var specialty = new Specialty { Name = request.Name };
            await _unitOfWork.Repository<Specialty>().AddAsync(specialty);
            await _unitOfWork.SaveChangesAsync();

            return specialty.Id;

        }
    }
}

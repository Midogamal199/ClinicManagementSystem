using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.UpdateDoctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDoctorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.DoctorRepository.GetByIdWithSpecialtiesAsync(request.Id);
            if (doctor is null)
            {
                throw new KeyNotFoundException($"Doctor with Id '{request.Id}' was not found.");
            }
            var specialties = await _unitOfWork.Repository<Specialty>().FindAsync(s => request.SpecialtyIds.Contains(s.Id));
            var specialtiesList = specialties.ToList();
            if (specialtiesList.Count != request.SpecialtyIds.Count)
            {
                throw new KeyNotFoundException("One or more specified specialties were not found.");
            }
            doctor.LicenseNumber=request.LicenseNumber;
            doctor.Specialties.Clear();
            foreach(var specialty in specialtiesList)
            {
                doctor.Specialties.Add(specialty);

            }
            _unitOfWork.Repository<Doctor>().Update(doctor);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

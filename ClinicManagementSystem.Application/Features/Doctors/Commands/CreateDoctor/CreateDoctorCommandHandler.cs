using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateDoctorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var employee =await _unitOfWork.Repository<Employee>().GetByIdAsync(request.EmployeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' was not found.");
            }
            var specialties = await _unitOfWork.Repository<Specialty>().FindAsync(s=>request.SpecialtyIds.Contains(s.Id));
            var specialtiesList = specialties.ToList();

            if (specialtiesList.Count != request.SpecialtyIds.Count)
            {
                throw new KeyNotFoundException("One or more specified specialties were not found.");
            }
            var doctor = new Doctor
            {
                LicenseNumber = request.LicenseNumber,
                EmployeeId = request.EmployeeId,
                Specialties = specialtiesList
            };
            await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return doctor.Id;


        }
    }
}

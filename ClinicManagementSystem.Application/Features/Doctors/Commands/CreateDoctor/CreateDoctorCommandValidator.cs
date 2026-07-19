using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommandValidator:AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator() {
        RuleFor(x=>x.LicenseNumber)
        .NotEmpty().WithMessage("License number is required.")
        .MaximumLength(50).WithMessage("License number cannot exceed 50 characters.");

            RuleFor(x => x.EmployeeId)
           .NotEmpty().WithMessage("Employee Id is required.");

            RuleFor(x => x.SpecialtyIds)
               .NotEmpty().WithMessage("At least one specialty must be selected.");



        }
    }
}

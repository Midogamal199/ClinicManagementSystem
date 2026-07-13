using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Patients.Commands.UpdatePatient
{
   public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator() {
            RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Patient Id is required.");
            RuleFor(x => x.FullName)
              .NotEmpty().WithMessage("Full name is required.")
              .MaximumLength(150).WithMessage("Full name cannot exceed 150 characters.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");
            RuleFor(x => x.Gender)
                            .IsInEnum().WithMessage("Invalid gender value.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
            RuleFor(x => x.Address)
              .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");







        }
    }
}

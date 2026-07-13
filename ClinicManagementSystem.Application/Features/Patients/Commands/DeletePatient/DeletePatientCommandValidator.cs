using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Patients.Commands.DeletePatient
{
    public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
    {
        public DeletePatientCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Patient Id is required.");
        }
    }
}

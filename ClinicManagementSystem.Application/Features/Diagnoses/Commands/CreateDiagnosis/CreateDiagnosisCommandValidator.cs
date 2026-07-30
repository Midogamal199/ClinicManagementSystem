using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Commands.CreateDiagnosis
{
    public class CreateDiagnosisCommandValidator: AbstractValidator<CreateDiagnosisCommand>
    {
        public CreateDiagnosisCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .NotEmpty().WithMessage("Visit Id is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.IcdCode)
                .MaximumLength(20).WithMessage("ICD code cannot exceed 20 characters.");
        }
    }
}

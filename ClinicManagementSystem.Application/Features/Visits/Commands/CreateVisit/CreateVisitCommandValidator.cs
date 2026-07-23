using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Visits.Commands.CreateVisit
{
    public class CreateVisitCommandValidator:AbstractValidator<CreateVisitCommand>
    {
        public CreateVisitCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment Id is required.");

            RuleFor(x => x.VisitDate)
                .NotEmpty().WithMessage("Visit date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Visit date cannot be in the future.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
        }
    }
}

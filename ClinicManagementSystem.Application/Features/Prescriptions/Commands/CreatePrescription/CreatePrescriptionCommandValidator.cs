using ClinicManagementSystem.Domain.Entities;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public class CreatePrescriptionCommandValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .NotEmpty().WithMessage("Visit Id is required.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one medicine item is required.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.MedicineName)
                    .NotEmpty().WithMessage("Medicine name is required.")
                    .MaximumLength(150).WithMessage("Medicine name cannot exceed 150 characters.");

                item.RuleFor(i => i.Dosage)
                    .NotEmpty().WithMessage("Dosage is required.")
                    .MaximumLength(100).WithMessage("Dosage cannot exceed 100 characters.");
                item.RuleFor(i => i.DurationInDays)
                    .InclusiveBetween(1, PrescriptionItem.MaxDurationInDays)
                    .WithMessage($"Duration must be between 1 and {PrescriptionItem.MaxDurationInDays} days.")
                    .When(i => i.DurationInDays.HasValue);
            });
        }
    }
}
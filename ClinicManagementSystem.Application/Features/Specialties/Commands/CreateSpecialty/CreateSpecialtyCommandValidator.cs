using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.CreateSpecialty
{
    public class CreateSpecialtyCommandValidator : AbstractValidator<CreateSpecialtyCommand>
    {
        public CreateSpecialtyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Specialty name is required.")
                .MaximumLength(100).WithMessage("Specialty name must not exceed 100 characters.");
        }
    }
}
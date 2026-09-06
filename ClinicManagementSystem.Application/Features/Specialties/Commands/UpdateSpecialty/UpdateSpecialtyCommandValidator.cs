using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Specialties.Commands.UpdateSpecialty
{
    public class UpdateSpecialtyCommandValidator : AbstractValidator<UpdateSpecialtyCommand>
    {
        public UpdateSpecialtyCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Specialty name is required.")
                .MaximumLength(100).WithMessage("Specialty name must not exceed 100 characters.");
        }
    }
}
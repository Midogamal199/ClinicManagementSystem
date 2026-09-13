using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches("[A-Za-z]").WithMessage("Password must contain at least one letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from the current password.");
        }
    }
}
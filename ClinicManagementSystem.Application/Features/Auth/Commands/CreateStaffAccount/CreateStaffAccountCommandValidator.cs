using System.Linq;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.CreateStaffAccount
{
    public class CreateStaffAccountCommandValidator : AbstractValidator<CreateStaffAccountCommand>
    {
        private static readonly string[] AllowedStaffRoles = { "Admin", "Doctor", "Receptionist" };

        public CreateStaffAccountCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => AllowedStaffRoles.Contains(role))
                .WithMessage($"Role must be one of: {string.Join(", ", AllowedStaffRoles)}.");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("EmployeeId is required for staff accounts.");

            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("LicenseNumber is required for Doctor accounts.")
                .When(x => x.Role == "Doctor");

            RuleFor(x => x.SpecialtyIds)
                .NotEmpty().WithMessage("At least one specialty is required for Doctor accounts.")
                .When(x => x.Role == "Doctor");
        }
    }
}
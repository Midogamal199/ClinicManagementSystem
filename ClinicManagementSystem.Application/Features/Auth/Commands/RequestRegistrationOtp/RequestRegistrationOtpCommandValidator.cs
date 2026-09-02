using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.RequestRegistrationOtp
{
    public class RequestRegistrationOtpCommandValidator : AbstractValidator<RequestRegistrationOtpCommand>
    {
        public RequestRegistrationOtpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
        }
    }
}
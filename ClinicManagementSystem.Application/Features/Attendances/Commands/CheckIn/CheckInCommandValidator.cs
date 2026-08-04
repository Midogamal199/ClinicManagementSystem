using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Attendances.Commands.CheckIn
{
    public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
    {
        public CheckInCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee Id is required.");
        }
    }
}
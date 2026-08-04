using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Attendances.Commands.CheckOut
{
    public class CheckOutCommandValidator : AbstractValidator<CheckOutCommand>
    {
        public CheckOutCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee Id is required.");
        }
    }
}
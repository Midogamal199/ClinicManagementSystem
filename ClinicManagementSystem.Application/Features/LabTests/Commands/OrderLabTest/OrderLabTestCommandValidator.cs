using FluentValidation;

namespace ClinicManagementSystem.Application.Features.LabTests.Commands.OrderLabTest
{
    public class OrderLabTestCommandValidator : AbstractValidator<OrderLabTestCommand>
    {
        public OrderLabTestCommandValidator()
        {
            RuleFor(x => x.VisitId).NotEmpty();

            RuleFor(x => x.TestType)
                .NotEmpty().WithMessage("Test type is required.")
                .MaximumLength(100);
        }
    }
}
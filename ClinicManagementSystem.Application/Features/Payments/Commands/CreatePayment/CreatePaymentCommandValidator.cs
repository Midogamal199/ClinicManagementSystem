using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("Invoice Id is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Payment amount must be greater than 0.");

            RuleFor(x => x.Method)
                .IsInEnum().WithMessage("Invalid payment method.");
        }
    }
}
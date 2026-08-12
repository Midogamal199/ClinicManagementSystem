using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.InitiateOnlinePayment
{
    public class InitiateOnlinePaymentCommandValidator : AbstractValidator<InitiateOnlinePaymentCommand>
    {
        public InitiateOnlinePaymentCommandValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("Invoice Id is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
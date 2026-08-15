using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.ConfirmOnlinePayment
{
    public class ConfirmOnlinePaymentCommandValidator: AbstractValidator<ConfirmOnlinePaymentCommand>
    {
        public ConfirmOnlinePaymentCommandValidator()
        {
            RuleFor(x => x.GatewayReference)
                .NotEmpty().WithMessage("Gateway reference is required.");

            RuleFor(x => x.RawPayload)
                .NotEmpty().WithMessage("Payload is required.");

            RuleFor(x => x.Signature)
                .NotEmpty().WithMessage("Signature is required.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Invoices.Commands.CreateInvoice
{
     public class CreateInvoiceCommandValidator: AbstractValidator<CreateInvoiceCommand>

    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient Id is required.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than 0.");
        }
    }
}

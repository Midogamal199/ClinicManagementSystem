using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.InitiateOnlinePayment
{
    public class InitiateOnlinePaymentCommand : IRequest<string>
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.ConfirmOnlinePayment
{
    public class ConfirmOnlinePaymentCommand: IRequest<Unit>
    {
        public string GatewayReference { get; set; }
        public bool IsSuccessful { get; set; }
        public string RawPayload { get; set; }
        public string Signature { get; set; }
    }
}

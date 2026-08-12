using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class FakePaymentGatewayService : IPaymentGatewayService
    {


        public Task<PaymentGatewayResult> CreateCheckoutSessionAsync(decimal amount, Guid transactionReference)
        {
            var fakeReference = $"FAKE-{transactionReference}";
            var fakeCheckoutUrl = $"https://fake-payment-gateway.test/checkout/{fakeReference}?amount={amount}";
            return Task.FromResult(new PaymentGatewayResult
            {
                Success = true,
                CheckoutUrl = fakeCheckoutUrl,
                GatewayReference = fakeReference
            });
        }
    }
}

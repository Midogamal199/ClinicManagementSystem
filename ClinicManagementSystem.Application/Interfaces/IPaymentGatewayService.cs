using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<PaymentGatewayResult> CreateCheckoutSessionAsync(decimal amount, Guid transactionReference);
    }

    public class PaymentGatewayResult
    {
        public bool Success { get; set; }
        public string? CheckoutUrl { get; set; }
        public string? GatewayReference { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

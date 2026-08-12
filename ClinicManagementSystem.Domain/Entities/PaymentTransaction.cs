using ClinicManagementSystem.Application.Common;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public string? GatewayReference { get; set; }
        public PaymentTransactionStatus Status { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public Guid? PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }
}
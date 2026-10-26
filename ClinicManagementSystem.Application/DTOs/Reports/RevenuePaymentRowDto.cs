using System;

namespace ClinicManagementSystem.Application.DTOs.Reports
{
    public class RevenuePaymentRowDto
    {
        public Guid PaymentId { get; set; }
        public DateTime PaidAt { get; set; }
        public Guid InvoiceId { get; set; }
        public string PatientFullName { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
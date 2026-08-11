namespace ClinicManagementSystem.Application.DTOs.Payments
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
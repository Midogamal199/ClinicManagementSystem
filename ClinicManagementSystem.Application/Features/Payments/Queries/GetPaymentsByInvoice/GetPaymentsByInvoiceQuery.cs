using ClinicManagementSystem.Application.DTOs.Payments;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoice
{
    public class GetPaymentsByInvoiceQuery : IRequest<List<PaymentDto>>
    {
        public Guid InvoiceId { get; set; }

        public GetPaymentsByInvoiceQuery(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
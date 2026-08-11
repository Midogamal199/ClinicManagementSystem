using ClinicManagementSystem.Application.DTOs.Invoices;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
    {
        public Guid Id { get; set; }

        public GetInvoiceByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
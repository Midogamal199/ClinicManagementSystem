using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Invoices;
using ClinicManagementSystem.Domain.Enums;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Queries.GetAllInvoices
{
    public class GetAllInvoicesQuery : IRequest<PaginatedResult<InvoiceDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? PatientId { get; set; }
        public InvoiceStatus? Status { get; set; }
    }
}
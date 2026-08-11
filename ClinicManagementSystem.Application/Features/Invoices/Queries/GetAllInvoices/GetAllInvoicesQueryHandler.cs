using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Invoices;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Queries.GetAllInvoices
{
    public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, PaginatedResult<InvoiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllInvoicesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            var (invoices, totalCount) = await _unitOfWork.InvoiceRepository.GetPagedWithDetailsAsync(
               request.PageNumber,
               request.PageSize,
               request.PatientId,
               request.Status);
            var dtos = invoices.Select(invoice =>
            {
                var paidAmount = invoice.Payments.Sum(p => p.Amount);
                return new InvoiceDto
                {
                    Id = invoice.Id,
                    TotalAmount = invoice.TotalAmount,
                    PaidAmount = paidAmount,
                    RemainingAmount = invoice.TotalAmount - paidAmount,
                    Status = invoice.Status.ToString(),
                    PatientId = invoice.PatientId,
                    PatientFullName = invoice.Patient.FullName
                };

            }).ToList();
            return new PaginatedResult<InvoiceDto>
            {
                Items = dtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}

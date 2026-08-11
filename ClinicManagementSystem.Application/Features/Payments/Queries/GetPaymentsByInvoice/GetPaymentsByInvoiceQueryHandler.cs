using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Payments;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoice
{
    public class GetPaymentsByInvoiceQueryHandler : IRequestHandler<GetPaymentsByInvoiceQuery, List<PaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPaymentsByInvoiceQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PaymentDto>> Handle(GetPaymentsByInvoiceQuery request, CancellationToken cancellationToken)
        {
            var payments = await _unitOfWork.Repository<Payment>()
                  .FindAsync(p => p.InvoiceId == request.InvoiceId);
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Method = p.Method.ToString(),
                InvoiceId = p.InvoiceId
            }).ToList();

        }
    }
}

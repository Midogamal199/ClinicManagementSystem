using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.Payments;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoice
{
    public class GetPaymentsByInvoiceQueryHandler : IRequestHandler<GetPaymentsByInvoiceQuery, List<PaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetPaymentsByInvoiceQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<List<PaymentDto>> Handle(GetPaymentsByInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with Id '{request.InvoiceId}' was not found.");
            }
            if (_currentUserService.IsInRole("Patient") && invoice.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to view payments for an invoice that does not belong to you.");
            }
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

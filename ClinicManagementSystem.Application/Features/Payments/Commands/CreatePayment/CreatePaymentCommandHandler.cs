using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdWithDetailsAsync(request.InvoiceId);
            if (invoice is null)
                throw new KeyNotFoundException($"Invoice with Id '{request.InvoiceId}' was not found.");

            if (invoice.Status == InvoiceStatus.Paid)
                throw new InvalidOperationException("This invoice has already been fully paid.");

            var alreadyPaid = invoice.Payments.Sum(p => p.Amount);
            var remaining = invoice.TotalAmount - alreadyPaid;

            if (request.Amount > remaining)
                throw new InvalidOperationException($"Payment amount ({request.Amount}) exceeds the remaining balance ({remaining}) on this invoice.");

           
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var payment = new Payment
                {
                    InvoiceId = request.InvoiceId,
                    Amount = request.Amount,
                    Method = request.Method
                };

                await _unitOfWork.Repository<Payment>().AddAsync(payment);

                var newTotalPaid = alreadyPaid + request.Amount;
                invoice.Status = newTotalPaid >= invoice.TotalAmount
                   ? InvoiceStatus.Paid
                   : InvoiceStatus.PartiallyPaid;

                _unitOfWork.Repository<Invoice>().Update(invoice);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return payment.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
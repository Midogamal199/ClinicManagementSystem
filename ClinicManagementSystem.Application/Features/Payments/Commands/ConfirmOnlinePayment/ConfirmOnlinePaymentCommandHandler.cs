using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Payments.Commands.ConfirmOnlinePayment
{
    public class ConfirmOnlinePaymentCommandHandler : IRequestHandler<ConfirmOnlinePaymentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebhookSignatureValidator _signatureValidator;

        public ConfirmOnlinePaymentCommandHandler(
            IUnitOfWork unitOfWork,
            IWebhookSignatureValidator signatureValidator)
        {
            _unitOfWork = unitOfWork;
            _signatureValidator = signatureValidator;
        }
        public async Task<Unit> Handle(ConfirmOnlinePaymentCommand request, CancellationToken cancellationToken)
        {
            var isSignatureValid= _signatureValidator.IsValid(request.RawPayload, request.Signature);
            if (!isSignatureValid)
            {
                throw new UnauthorizedAccessException("Invalid webhook signature.");
            }
            var transactions = await _unitOfWork.Repository<PaymentTransaction>().FindAsync(t => t.GatewayReference == request.GatewayReference);
            var transaction = transactions.FirstOrDefault();
            if (transaction is null)
            {
                throw new KeyNotFoundException(
                    $"No payment transaction found with gateway reference '{request.GatewayReference}'.");
            }
            if (transaction.Status != PaymentTransactionStatus.Pending)
            {
                throw new InvalidOperationException(
                    $"This transaction has already been processed with status '{transaction.Status}'.");
            }
            if(!request.IsSuccessful)
            {
               transaction.Status = PaymentTransactionStatus.Failed;
                _unitOfWork.Repository<PaymentTransaction>().Update(transaction);
                await _unitOfWork.SaveChangesAsync();
                return Unit.Value;
            }
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdWithDetailsAsync(transaction.InvoiceId);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with Id '{transaction.InvoiceId}' was not found.");
            }
            var payment = new Payment
            {
                InvoiceId = transaction.InvoiceId,
                Amount = transaction.Amount,
                Method = PaymentMethod.Visa
            };
            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            transaction.Status = PaymentTransactionStatus.Success;
            transaction.PaymentId = payment.Id;
            _unitOfWork.Repository<PaymentTransaction>().Update(transaction);
            var alreadyPaid = invoice.Payments.Sum(p => p.Amount) + transaction.Amount;
            invoice.Status = alreadyPaid >= invoice.TotalAmount
               ? InvoiceStatus.Paid
               : InvoiceStatus.PartiallyPaid;
            _unitOfWork.Repository<Invoice>().Update(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;

        }
    }
}

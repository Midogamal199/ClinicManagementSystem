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

namespace ClinicManagementSystem.Application.Features.Payments.Commands.InitiateOnlinePayment
{
    public class InitiateOnlinePaymentCommandHandler : IRequestHandler<InitiateOnlinePaymentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public InitiateOnlinePaymentCommandHandler(
            IUnitOfWork unitOfWork,
            IPaymentGatewayService paymentGatewayService)
        {
            _unitOfWork = unitOfWork;
            _paymentGatewayService = paymentGatewayService;
        }
        public async Task<string> Handle(InitiateOnlinePaymentCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdWithDetailsAsync(request.InvoiceId);
            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with Id '{request.InvoiceId}' was not found.");
            }
            if (invoice.Status == InvoiceStatus.Paid)
            {
                throw new InvalidOperationException("This invoice has already been fully paid.");
            }
            var alreadyPaid = invoice.Payments.Sum(p => p.Amount);
            var remaining = invoice.TotalAmount - alreadyPaid;
            if (request.Amount > remaining)
            {
                throw new InvalidOperationException(
                    $"Payment amount ({request.Amount}) exceeds the remaining balance ({remaining}) on this invoice.");
            }

            var transaction = new PaymentTransaction
            {
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                Status = PaymentTransactionStatus.Pending
            };
            await _unitOfWork.Repository<PaymentTransaction>().AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            var gatewayResult = await _paymentGatewayService.CreateCheckoutSessionAsync(
                request.Amount, transaction.Id);
            if (!gatewayResult.Success)
            {
                transaction.Status = PaymentTransactionStatus.Failed;
                _unitOfWork.Repository<PaymentTransaction>().Update(transaction);
                await _unitOfWork.SaveChangesAsync();

                throw new InvalidOperationException(
                    $"Failed to initiate payment: {gatewayResult.ErrorMessage}");
            }
            transaction.GatewayReference = gatewayResult.GatewayReference;
            _unitOfWork.Repository<PaymentTransaction>().Update(transaction);
            await _unitOfWork.SaveChangesAsync();

            return gatewayResult.CheckoutUrl!;

        }
    }
}

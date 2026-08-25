using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Payments.Commands.InitiateOnlinePayment;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using Moq;

namespace ClinicManagementSystem.Application.Tests.Payments
{
    public class InitiateOnlinePaymentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPaymentGatewayService> _paymentGatewayServiceMock;
        private readonly InitiateOnlinePaymentCommandHandler _handler;
        public InitiateOnlinePaymentCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _paymentGatewayServiceMock = new Mock<IPaymentGatewayService>();
            _handler = new InitiateOnlinePaymentCommandHandler(
                _unitOfWorkMock.Object,
                _paymentGatewayServiceMock.Object);
        }
        [Fact]
        public async Task Handle_InvoiceNotFound_ThrowsKeyNotFoundException()
        {
            var command = new InitiateOnlinePaymentCommand
            {
                InvoiceId = Guid.NewGuid(),
                Amount = 500m
            };
            _unitOfWorkMock.Setup(u => u.InvoiceRepository.GetByIdWithDetailsAsync(command.InvoiceId))
                .ReturnsAsync((Invoice)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(
              () => _handler.Handle(command, CancellationToken.None));



        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsCheckoutUrl()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice
            {
                Id = invoiceId,
                TotalAmount = 500m,
                Status = InvoiceStatus.Unpaid,
                Payments = new List<Payment>()
            };
            var command = new InitiateOnlinePaymentCommand
            {
                InvoiceId = invoiceId,
                Amount = 500m
            };
            _unitOfWorkMock
       .Setup(u => u.InvoiceRepository.GetByIdWithDetailsAsync(invoiceId))
       .ReturnsAsync(invoice);
            var transactionRepoMock = new Mock<IGenericRepository<PaymentTransaction>>();
            _unitOfWorkMock
     .Setup(u => u.Repository<PaymentTransaction>())
     .Returns(transactionRepoMock.Object);

            _paymentGatewayServiceMock
                .Setup(g => g.CreateCheckoutSessionAsync(command.Amount, It.IsAny<Guid>()))
                .ReturnsAsync(new PaymentGatewayResult
                {
                    Success = true,
                    CheckoutUrl = "https://accept.paymob.com/fake-checkout",
                    GatewayReference = "593517504"
                });
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.Equal("https://accept.paymob.com/fake-checkout", result);
            transactionRepoMock.Verify(r => r.AddAsync(It.IsAny<PaymentTransaction>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));
        }
        [Fact]
        public async Task Handle_InvoiceAlreadyPaid_ThrowsInvalidOperationException()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice
            {
                Id = invoiceId,
                TotalAmount = 500m,
                Status = InvoiceStatus.Paid,
                Payments = new List<Payment>()
            };

            var command = new InitiateOnlinePaymentCommand
            {
                InvoiceId = invoiceId,
                Amount = 500m
            };

            _unitOfWorkMock
                .Setup(u => u.InvoiceRepository.GetByIdWithDetailsAsync(invoiceId))
                .ReturnsAsync(invoice);
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
       () => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("This invoice has already been fully paid.", exception.Message);
        }
        [Fact]
        public async Task Handle_AmountExceedsRemainingBalance_ThrowsInvalidOperationException()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice
            {
                Id = invoiceId,
                TotalAmount = 500m,
                Status = InvoiceStatus.Unpaid,
                Payments = new List<Payment>
        {
            new Payment { Amount = 400m }
        }
            };
            var command = new InitiateOnlinePaymentCommand
            {
                InvoiceId = invoiceId,
                Amount = 200m 
            };
            _unitOfWorkMock
       .Setup(u => u.InvoiceRepository.GetByIdWithDetailsAsync(invoiceId))
       .ReturnsAsync(invoice);

            
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Commands.CreateInvoice
{
    internal class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateInvoiceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(request.PatientId);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Patient with Id '{request.PatientId}' was not found.");
            }
            var invoice = new Invoice
            {
                PatientId = request.PatientId,
                TotalAmount = request.TotalAmount,
                Status = InvoiceStatus.Unpaid
            };
            await _unitOfWork.Repository<Invoice>().AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return invoice.Id;
        }
    }
}

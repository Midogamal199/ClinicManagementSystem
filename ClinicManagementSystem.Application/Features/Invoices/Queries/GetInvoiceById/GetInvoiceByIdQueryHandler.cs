using ClinicManagementSystem.Application.DTOs.Invoices;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInvoiceByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdWithDetailsAsync(request.Id);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with Id '{request.Id}' was not found.");
            }

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
        }
    }
}
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.Invoices;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetInvoiceByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdWithDetailsAsync(request.Id);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with Id '{request.Id}' was not found.");
            }
            if (_currentUserService.IsInRole("Patient") && invoice.PatientId != _currentUserService.PatientId)
            {
                throw new ForbiddenAccessException("You are not allowed to view an invoice that does not belong to you.");
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
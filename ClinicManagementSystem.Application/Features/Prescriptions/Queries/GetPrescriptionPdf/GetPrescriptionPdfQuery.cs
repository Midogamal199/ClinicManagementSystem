using System;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionPdf
{
    public class GetPrescriptionPdfQuery : IRequest<PrescriptionPdfResult>
    {
        public Guid Id { get; set; }

        public GetPrescriptionPdfQuery(Guid id)
        {
            Id = id;
        }
    }
}
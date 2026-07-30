using ClinicManagementSystem.Application.DTOs.Prescriptions;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById
{
    public class GetPrescriptionByIdQuery : IRequest<PrescriptionDto>
    {
        public Guid Id { get; set; }

        public GetPrescriptionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
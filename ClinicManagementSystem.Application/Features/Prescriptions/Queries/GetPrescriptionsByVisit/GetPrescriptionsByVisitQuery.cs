using ClinicManagementSystem.Application.DTOs.Prescriptions;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionsByVisit
{
    public class GetPrescriptionsByVisitQuery : IRequest<List<PrescriptionDto>>
    {
        public Guid VisitId { get; set; }

        public GetPrescriptionsByVisitQuery(Guid visitId)
        {
            VisitId = visitId;
        }
    }
}
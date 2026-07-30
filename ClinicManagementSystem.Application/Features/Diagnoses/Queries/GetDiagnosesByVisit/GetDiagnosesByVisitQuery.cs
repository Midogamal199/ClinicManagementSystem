using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Diagnoses;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Queries.GetDiagnosesByVisit
{
    public class GetDiagnosesByVisitQuery: IRequest<List<DiagnosisDto>>
    {
        public Guid VisitId { get; set; }

        public GetDiagnosesByVisitQuery(Guid visitId)
        {
            VisitId = visitId;
        }
    }
}

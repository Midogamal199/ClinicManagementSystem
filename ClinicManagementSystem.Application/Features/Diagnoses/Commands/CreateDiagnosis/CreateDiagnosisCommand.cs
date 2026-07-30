using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Commands.CreateDiagnosis
{
    public class CreateDiagnosisCommand:IRequest<Guid>
    {
        public Guid VisitId { get; set; }
        public string Description { get; set; }
        public string IcdCode { get; set; }
    }
}

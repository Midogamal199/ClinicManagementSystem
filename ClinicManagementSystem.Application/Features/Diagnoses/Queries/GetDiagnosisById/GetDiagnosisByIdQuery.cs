using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Diagnoses;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Queries.GetDiagnosisById
{
    public class GetDiagnosisByIdQuery: IRequest<DiagnosisDto>

    {
        public Guid Id { get; set; }

        public GetDiagnosisByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}

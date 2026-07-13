using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Patients;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Queries.GetPatientById
{
    public class GetPatientByIdQuery : IRequest<PatientDto>
    {
        public Guid Id { get; set; }

        public GetPatientByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

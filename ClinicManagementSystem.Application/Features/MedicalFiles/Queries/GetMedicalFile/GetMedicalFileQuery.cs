using System;
using ClinicManagementSystem.Application.DTOs.MedicalFiles;
using MediatR;

namespace ClinicManagementSystem.Application.Features.MedicalFiles.Queries.GetMedicalFile
{
    public class GetMedicalFileQuery : IRequest<MedicalFileDto>
    {
        public Guid PatientId { get; set; }

        public GetMedicalFileQuery(Guid patientId)
        {
            PatientId = patientId;
        }
    }
}
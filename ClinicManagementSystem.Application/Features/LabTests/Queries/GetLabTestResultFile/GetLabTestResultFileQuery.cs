using System;
using ClinicManagementSystem.Application.DTOs.LabTests;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LabTests.Queries.GetLabTestResultFile
{
    public class GetLabTestResultFileQuery : IRequest<LabTestFileResult>
    {
        public Guid Id { get; set; }

        public GetLabTestResultFileQuery(Guid id)
        {
            Id = id;
        }
    }
}
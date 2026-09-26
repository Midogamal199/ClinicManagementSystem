using System;
using ClinicManagementSystem.Application.DTOs.LabTests;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LabTests.Queries.GetLabTestById
{
    public class GetLabTestByIdQuery : IRequest<LabTestDto>
    {
        public Guid Id { get; set; }

        public GetLabTestByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
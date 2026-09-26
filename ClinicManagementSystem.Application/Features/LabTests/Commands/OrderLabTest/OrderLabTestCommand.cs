using System;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LabTests.Commands.OrderLabTest
{
    public class OrderLabTestCommand : IRequest<Guid>
    {
        public Guid VisitId { get; set; }
        public string TestType { get; set; } = string.Empty;
    }
}
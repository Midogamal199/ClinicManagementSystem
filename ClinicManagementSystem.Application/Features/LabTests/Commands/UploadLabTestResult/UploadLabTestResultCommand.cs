using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ClinicManagementSystem.Application.Features.LabTests.Commands.UploadLabTestResult
{
    public class UploadLabTestResultCommand : IRequest<Unit>
    {
        public Guid LabTestId { get; set; }
        public IFormFile ResultFile { get; set; }
    }
}

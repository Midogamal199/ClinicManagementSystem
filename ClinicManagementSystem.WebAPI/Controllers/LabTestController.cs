using ClinicManagementSystem.Application.Features.LabTests.Commands.OrderLabTest;
using ClinicManagementSystem.Application.Features.LabTests.Commands.UploadLabTestResult;
using ClinicManagementSystem.Application.Features.LabTests.Queries.GetLabTestById;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LabTestController: ControllerBase
    {
        private readonly IMediator _mediator;

        public LabTestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Roles = Roles.Doctor)]
        public async Task<IActionResult> Order([FromBody] OrderLabTestCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [HttpPost("{id}/upload-result")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Receptionist}")]
        public async Task<IActionResult> UploadResult(Guid id, IFormFile resultFile)
        {
            await _mediator.Send(new UploadLabTestResultCommand { LabTestId = id, ResultFile = resultFile });
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetLabTestByIdQuery(id));
            return Ok(result);
        }
    }
}

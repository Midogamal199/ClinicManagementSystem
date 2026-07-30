using ClinicManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription;
using ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById;
using ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionsByVisit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionCommand command)
        {
            var prescriptionId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = prescriptionId }, new { id = prescriptionId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var prescription = await _mediator.Send(new GetPrescriptionByIdQuery(id));
            return Ok(prescription);
        }

        [HttpGet("by-visit/{visitId}")]
        public async Task<IActionResult> GetByVisit(Guid visitId)
        {
            var prescriptions = await _mediator.Send(new GetPrescriptionsByVisitQuery(visitId));
            return Ok(prescriptions);
        }
    }
}

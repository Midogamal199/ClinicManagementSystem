using ClinicManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription;
using ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById;
using ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionPdf;
using ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionsByVisit;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class PrescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor}")]

        public async Task<IActionResult> Create([FromBody] CreatePrescriptionCommand command)
        {
            var prescriptionId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = prescriptionId }, new { id = prescriptionId });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor},{Roles.Receptionist}")]

        public async Task<IActionResult> GetById(Guid id)
        {
            var prescription = await _mediator.Send(new GetPrescriptionByIdQuery(id));
            return Ok(prescription);
        }

        [HttpGet("by-visit/{visitId}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor},{Roles.Receptionist}")]

        public async Task<IActionResult> GetByVisit(Guid visitId)
        {
            var prescriptions = await _mediator.Send(new GetPrescriptionsByVisitQuery(visitId));
            return Ok(prescriptions);
        }
        [HttpGet("{id}/pdf")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor},{Roles.Receptionist},{Roles.Patient}")]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            var result = await _mediator.Send(new GetPrescriptionPdfQuery(id));
            return File(result.Content, "application/pdf", result.FileName);
        }
    }
}

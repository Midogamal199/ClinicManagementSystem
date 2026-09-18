using ClinicManagementSystem.Application.Features.Diagnoses.Commands.CreateDiagnosis;
using ClinicManagementSystem.Application.Features.Diagnoses.Queries.GetDiagnosesByVisit;
using ClinicManagementSystem.Application.Features.Diagnoses.Queries.GetDiagnosisById;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor},{Roles.Receptionist}")]

    public class DiagnosisController:ControllerBase
    {
        private readonly IMediator _mediator;

        public DiagnosisController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor}")]

        public async Task<IActionResult> Create([FromBody] CreateDiagnosisCommand command)
        {
            var diagnosisId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = diagnosisId }, new { id = diagnosisId });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var diagnosis = await _mediator.Send(new GetDiagnosisByIdQuery(id));
            return Ok(diagnosis);
        }
        [HttpGet("by-visit/{visitId}")]
        public async Task<IActionResult> GetByVisit(Guid visitId)
        {
            var diagnoses = await _mediator.Send(new GetDiagnosesByVisitQuery(visitId));
            return Ok(diagnoses);
        }

    }
}

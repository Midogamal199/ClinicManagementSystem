using ClinicManagementSystem.Application.Features.Patients.Commands.CreatePatient;
using ClinicManagementSystem.Application.Features.Patients.Commands.DeletePatient;
using ClinicManagementSystem.Application.Features.Patients.Commands.UpdatePatient;
using ClinicManagementSystem.Application.Features.Patients.Queries.GetAllPatients;
using ClinicManagementSystem.Application.Features.Patients.Queries.GetPatientById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
        [Route("api/[controller]")]

    public class PatientController: ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientCommand command) 
        {
            var patientId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = patientId }, new { id = patientId });


        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Route Id does not match body Id.");
            }
            await _mediator.Send(command);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeletePatientCommand { Id = id });
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) 
        
        { 
        var patient=await _mediator.Send(new GetPatientByIdQuery(id));
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        
        
        
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPatientsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}

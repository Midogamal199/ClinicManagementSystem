using ClinicManagementSystem.Application.Features.Doctors.Commands.CreateDoctor;
using ClinicManagementSystem.Application.Features.Doctors.Commands.DeleteDoctor;
using ClinicManagementSystem.Application.Features.Doctors.Commands.UpdateDoctor;
using ClinicManagementSystem.Application.Features.Doctors.Queries.GetAllDoctors;
using ClinicManagementSystem.Application.Features.Doctors.Queries.GetDoctorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = doctorId }, new { id = doctorId });

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDoctorCommand command)
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
            var doctor = await _mediator.Send(new DeleteDoctorCommand { Id = id });
            return NoContent();

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var doctor = await _mediator.Send(new GetDoctorByIdQuery(id));
            return Ok(doctor);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDoctorsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
            
        }
    }
}

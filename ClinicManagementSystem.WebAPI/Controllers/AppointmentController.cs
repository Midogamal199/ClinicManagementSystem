using ClinicManagementSystem.Application.Features.Appointments.Commands.CreateAppointment;
using ClinicManagementSystem.Application.Features.Appointments.Commands.DeleteAppointment;
using ClinicManagementSystem.Application.Features.Appointments.Commands.UpdateAppointment;
using ClinicManagementSystem.Application.Features.Appointments.Queries.GetAllAppointments;
using ClinicManagementSystem.Application.Features.Appointments.Queries.GetAppointmentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
        {
            var appointmentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = appointmentId }, new { id = appointmentId });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentCommand command)
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
            await _mediator.Send(new DeleteAppointmentCommand { Id = id });
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var appointment = await _mediator.Send(new GetAppointmentByIdQuery(id));
            return Ok(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAppointmentsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }



    }
}

using ClinicManagementSystem.Application.Features.Attendances.Commands.CheckIn;
using ClinicManagementSystem.Application.Features.Attendances.Commands.CheckOut;
using ClinicManagementSystem.Application.Features.Attendances.Queries.GetAllAttendances;
using ClinicManagementSystem.Application.Features.Attendances.Queries.GetAttendanceById;
using ClinicManagementSystem.Application.Features.Attendances.Queries.GetWorkingHours;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command)
        {
            var attendanceId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = attendanceId }, new { id = attendanceId });
        }
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var attendance = await _mediator.Send(new GetAttendanceByIdQuery(id));
            return Ok(attendance);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAttendancesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{id}/working-hours")]
        public async Task<IActionResult> GetWorkingHours(Guid id)
        {
            var hours = await _mediator.Send(new GetWorkingHoursQuery(id));
            return Ok(new { workingHours = hours });
        }


    }
}

using ClinicManagementSystem.Application.Features.Employees.Commands.CreateEmployee;
using ClinicManagementSystem.Application.Features.Employees.Commands.DeleteEmployee;
using ClinicManagementSystem.Application.Features.Employees.Commands.UpdateEmployeeCommand.cs;
using ClinicManagementSystem.Application.Features.Employees.Queries.GetAllEmployees;
using ClinicManagementSystem.Application.Features.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController: ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            var employeeId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = employeeId }, new { id = employeeId });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
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
            await _mediator.Send(new DeleteEmployeeCommand { Id = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return Ok(employee);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllEmployeesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}

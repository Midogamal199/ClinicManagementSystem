using ClinicManagementSystem.Application.Features.Departments.Commands.CreateDepartment;
using ClinicManagementSystem.Application.Features.Departments.Commands.DeleteDepartment;
using ClinicManagementSystem.Application.Features.Departments.Commands.UpdateDepartment;
using ClinicManagementSystem.Application.Features.Departments.Queries.GetAllDepartments;
using ClinicManagementSystem.Application.Features.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command)
        {
            var departmentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = departmentId }, new { id = departmentId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentCommand command)
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
            await _mediator.Send(new DeleteDepartmentCommand { Id = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var department = await _mediator.Send(new GetDepartmentByIdQuery(id));
            return Ok(department);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDepartmentsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
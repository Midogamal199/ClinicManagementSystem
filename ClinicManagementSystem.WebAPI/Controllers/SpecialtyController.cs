using System;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Specialties.Commands.CreateSpecialty;
using ClinicManagementSystem.Application.Features.Specialties.Commands.DeleteSpecialty;
using ClinicManagementSystem.Application.Features.Specialties.Commands.UpdateSpecialty;
using ClinicManagementSystem.Application.Features.Specialties.Queries.GetAllSpecialties;
using ClinicManagementSystem.Application.Features.Specialties.Queries.GetDoctorsBySpecialty;
using ClinicManagementSystem.Application.Features.Specialties.Queries.GetSpecialtyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/Specialization")]
    public class SpecialtyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecialtyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSpecialtiesQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpecialtyCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateSpecialtyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Route id does not match body id.");
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteSpecialtyCommand { Id = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetSpecialtyByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpGet("{id}/doctors")]
        public async Task<IActionResult> GetDoctors(Guid id)
        {
            var result = await _mediator.Send(new GetDoctorsBySpecialtyQuery { SpecialtyId = id });
            return Ok(result);
        }
    }
}
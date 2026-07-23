using ClinicManagementSystem.Application.Features.Visits.Commands.CreateVisit;
using ClinicManagementSystem.Application.Features.Visits.Queries.GetAllVisitsQuery;
using ClinicManagementSystem.Application.Features.Visits.Queries.GetVisitById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitController: ControllerBase
    {
        private readonly IMediator _mediator;

        public VisitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVisitCommand command)
        {
            var visitId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = visitId }, new { id = visitId });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var visit = await _mediator.Send(new GetVisitByIdQuery(id));
            return Ok(visit);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllVisitsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}

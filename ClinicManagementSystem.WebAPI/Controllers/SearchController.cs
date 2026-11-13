using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Search.Queries.GlobalSearch;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor},{Roles.Receptionist}")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] GlobalSearchQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
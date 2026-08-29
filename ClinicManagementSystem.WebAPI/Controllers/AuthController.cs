using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Auth.Commands.CreateStaffAccount;
using ClinicManagementSystem.Application.Features.Auth.Commands.Register;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(new { userId });
        }
        [HttpPost("create-staff")]
        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> CreateStaffAccount(CreateStaffAccountCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(new { userId });
        }
    }
}
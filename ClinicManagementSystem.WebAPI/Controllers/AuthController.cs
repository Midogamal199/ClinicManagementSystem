using System.Security.Claims;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Auth.Commands.ChangePassword;
using ClinicManagementSystem.Application.Features.Auth.Commands.CreateStaffAccount;
using ClinicManagementSystem.Application.Features.Auth.Commands.Login;
using ClinicManagementSystem.Application.Features.Auth.Commands.RefreshToken;
using ClinicManagementSystem.Application.Features.Auth.Commands.RequestRegistrationOtp;
using ClinicManagementSystem.Application.Features.Auth.Commands.VerifyRegistration;
using ClinicManagementSystem.Infrastructure.Identity;
using ClinicManagementSystem.WebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        [HttpPost("register/request-otp")]
        public async Task<IActionResult> RequestRegistrationOtp(RequestRegistrationOtpCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Verification code sent to your email." });
        }
        [HttpPost("register/verify")]
        public async Task<IActionResult> VerifyRegistration(VerifyRegistrationCommand command)
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value;
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }
            var command = new ChangePasswordCommand
            {
                UserId = userId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };
            await _mediator.Send(command);
            return NoContent();
        }

    }
}
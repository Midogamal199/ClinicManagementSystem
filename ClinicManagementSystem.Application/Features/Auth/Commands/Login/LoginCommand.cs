using ClinicManagementSystem.Application.DTOs.Auth;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
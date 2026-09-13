using ClinicManagementSystem.Application.DTOs.Auth;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginResponseDto>
    {
        public string RefreshToken { get; set; }
    }
}
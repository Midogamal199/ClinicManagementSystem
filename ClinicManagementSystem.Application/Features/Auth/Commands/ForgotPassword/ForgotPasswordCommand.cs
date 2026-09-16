using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}
using System;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; } 
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
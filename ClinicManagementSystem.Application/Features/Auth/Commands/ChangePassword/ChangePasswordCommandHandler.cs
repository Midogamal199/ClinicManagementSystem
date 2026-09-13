using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var resualt = await _identityService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
            if (!resualt.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", resualt.Errors));
            }
            return Unit.Value;
        }
    }
}

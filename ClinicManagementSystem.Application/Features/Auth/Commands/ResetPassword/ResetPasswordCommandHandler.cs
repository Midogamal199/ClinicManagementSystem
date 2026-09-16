using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IIdentityService _identityService;
        private readonly IOtpService _otpService;

        public ResetPasswordCommandHandler(IIdentityService identityService, IOtpService otpService)
        {
            _identityService = identityService;
            _otpService = otpService;
        }
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var isOtpValid = _otpService.ValidateOtp(request.Email, request.Otp);
            if (!isOtpValid)
            {
                throw new InvalidOperationException("Invalid or expired verification code.");
            }
            var result = await _identityService.ResetPasswordAsync(request.Email, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to reset password: {string.Join("; ", result.Errors)}");
            }
            var userId = await _identityService.GetUserIdByEmailAsync(request.Email);
            if (userId.HasValue)
            {
                await _identityService.RevokeAllUserTokensAsync(userId.Value);
            }
            return Unit.Value;
        }
    }
}

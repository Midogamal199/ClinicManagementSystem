using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IIdentityService _identityService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IIdentityService identityService,
            IOtpService otpService,
            IEmailService emailService)
        {
            _identityService = identityService;
            _otpService = otpService;
            _emailService = emailService;
        }
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _identityService.UserExistsAsync(request.Email);
            if (userExists)
            {
                var otp = await _otpService.GenerateAndStoreOtpAsync(request.Email);

                await _emailService.SendEmailAsync(
                    request.Email,
                    "Reset your Clinic Management System password",
                    $"Your password reset code is: {otp}\nIt expires in 10 minutes.\n\nIf you didn't request this, please ignore this email.");
            }
            return Unit.Value;
        }
    }
}

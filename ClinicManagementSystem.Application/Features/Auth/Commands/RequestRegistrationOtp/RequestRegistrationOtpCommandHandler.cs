using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.RequestRegistrationOtp
{
    public class RequestRegistrationOtpCommandHandler : IRequestHandler<RequestRegistrationOtpCommand, Unit>
    {
        private readonly IIdentityService _identityService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public RequestRegistrationOtpCommandHandler(
            IIdentityService identityService,
            IOtpService otpService,
            IEmailService emailService)
        {
            _identityService = identityService;
            _otpService = otpService;
            _emailService = emailService;
        }
        public async Task<Unit> Handle(RequestRegistrationOtpCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _identityService.UserExistsAsync(request.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");
            }
            var otp = await _otpService.GenerateAndStoreOtpAsync(request.Email);
            await _emailService.SendEmailAsync(
              request.Email,
              "Your Clinic Management System verification code",
              $"Your verification code is: {otp}\nIt expires in 10 minutes.");
            return Unit.Value;

        }
    }
}

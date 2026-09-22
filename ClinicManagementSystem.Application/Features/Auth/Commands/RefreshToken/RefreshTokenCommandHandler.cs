using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Auth;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }
        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.ValidateAndConsumeRefreshTokenAsync(request.RefreshToken);
            if (userId is null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            var userInfo = await _identityService.GetUserInfoAsync(userId.Value);
            if (!userInfo.Succeeded)
            {
                throw new UnauthorizedAccessException("User no longer exists.");
            }
            var generatedToken = await _tokenService.GenerateAccessTokenAsync(userId.Value, userInfo.Email, userInfo.Roles, userInfo.PatientId, userInfo.EmployeeId, userInfo.DoctorId);
            var (newRefreshToken, refreshTokenExpiresAt) = _tokenService.GenerateRefreshToken();
            await _identityService.StoreRefreshTokenAsync(userId.Value, newRefreshToken, refreshTokenExpiresAt);
            return new LoginResponseDto
            {
                Token = generatedToken.Token,
                ExpiresAt = generatedToken.ExpiresAt,
                RefreshToken = newRefreshToken,
                UserId = userId.Value,
                Email = userInfo.Email,
                FullName = userInfo.FullName,
                Roles = userInfo.Roles
            };
        }
    }
}

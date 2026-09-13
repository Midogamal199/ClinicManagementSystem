using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Auth;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var resualt = await _identityService.LoginAsync(request.Email, request.Password);
            if (!resualt.Succeeded)
            {
                throw new UnauthorizedAccessException(string.Join("; ", resualt.Errors));
            }
            var userId= Guid.Parse(resualt.UserId);
            var generatedToken=await _tokenService.GenerateAccessTokenAsync(userId, request.Email, resualt.Roles);
            var (refreshToken, refreshTokenExpiresAt) = _tokenService.GenerateRefreshToken();
            await _identityService.StoreRefreshTokenAsync(userId, refreshToken, refreshTokenExpiresAt);


            return new LoginResponseDto
            {
                Token = generatedToken.Token,
                ExpiresAt = generatedToken.ExpiresAt,
                RefreshToken = refreshToken,
                UserId = userId,
                Email = request.Email,
                FullName = resualt.FullName,
                Roles = resualt.Roles
            };



        }
    }
}

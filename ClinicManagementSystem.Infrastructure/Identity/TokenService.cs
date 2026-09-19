using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ClinicManagementSystem.Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _options;

        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public  Task<GeneratedToken> GenerateAccessTokenAsync(Guid userId, string email, IList<string> roles, Guid? patientId, Guid? employeeId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (patientId is not null)
            {
                claims.Add(new Claim("PatientId", patientId.Value.ToString()));
            }

            if (employeeId is not null)
            {
                claims.Add(new Claim("EmployeeId", employeeId.Value.ToString()));
            }
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes);
            var token =new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
           return  Task.FromResult(new GeneratedToken
            {
                Token = tokenString,
                ExpiresAt = expiresAt
            });
        }

        public (string Token, DateTime ExpiresAt) GenerateRefreshToken()
        {

            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            var expiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);
            return (token, expiresAt);
        }
    }
}

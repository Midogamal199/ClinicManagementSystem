using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public class GeneratedToken
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public interface ITokenService
    {
        Task<GeneratedToken> GenerateAccessTokenAsync(Guid userId, string email, IList<string> roles);
    }
}
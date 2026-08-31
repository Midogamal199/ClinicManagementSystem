using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class OtpService : IOtpService
    {

        private readonly IMemoryCache _cache;
        private static readonly TimeSpan OtpLifetime = TimeSpan.FromMinutes(10);

        public OtpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<string> GenerateAndStoreOtpAsync(string email)
        {
            var code = Random.Shared.Next(100000, 999999).ToString();
            _cache.Set(GetCacheKey(email), code, OtpLifetime);
            return Task.FromResult(code);
        }

        public bool ValidateOtp(string email, string code)
        {
            if (_cache.TryGetValue(GetCacheKey(email), out string? storedCode))
            {
                var isValid = storedCode == code;
                if (isValid)
                {
                    _cache.Remove(GetCacheKey(email)); 
                }
                return isValid;
            }
            return false;
        }
        private static string GetCacheKey(string email) => $"otp:{email.ToLower()}";

    }
}

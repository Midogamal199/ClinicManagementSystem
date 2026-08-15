using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class FakeWebhookSignatureValidator : IWebhookSignatureValidator
    {
        private const string SecretKey = "fake-secret-key-for-testing-only";
        public bool IsValid(string payload, string receivedSignature)
        {
            var computedSignature = ComputeHmacSha256(payload, SecretKey);
            return computedSignature == receivedSignature;
        }
        private static string ComputeHmacSha256(string payload, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);

            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}

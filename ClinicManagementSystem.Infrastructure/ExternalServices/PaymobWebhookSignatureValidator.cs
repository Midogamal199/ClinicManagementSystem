using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class PaymobWebhookSignatureValidator : IWebhookSignatureValidator
    {
        private readonly PaymobOptions _options;
        public PaymobWebhookSignatureValidator(IOptions<PaymobOptions> options)
        {
            _options = options.Value;
        }
        private static readonly string[] FieldOrder =
       {
            "amount_cents", "created_at", "currency", "error_occured",
            "has_parent_transaction", "id", "integration_id", "is_3d_secure",
            "is_auth", "is_capture", "is_refunded", "is_standalone_payment",
            "is_voided", "order.id", "owner", "pending",
            "source_data.pan", "source_data.sub_type", "source_data.type", "success"
        };
        public bool IsValid(string payload, string receivedSignature)
        {
            using var document = JsonDocument.Parse(payload);
            var root = document.RootElement.TryGetProperty("obj", out var obj) ? obj : document.RootElement;
            var concatenated = new StringBuilder();
            foreach (var field in FieldOrder)
            {
                var value = GetNestedValue(root, field);
                concatenated.Append(value);
            }
            var computedSignature = ComputeHmacSha512(concatenated.ToString(), _options.HmacSecret);

            return string.Equals(computedSignature, receivedSignature, StringComparison.OrdinalIgnoreCase);

        }
        private static string GetNestedValue(JsonElement root, string fieldPath)
        {
            var parts = fieldPath.Split('.');
            var current = root;

            foreach (var part in parts)
            {
                if (!current.TryGetProperty(part, out var next))
                {
                    return string.Empty;
                }
                current = next;
            }

            return current.ValueKind switch
            {
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Null => string.Empty,
                _ => current.ToString()
            };
        }
        private static string ComputeHmacSha512(string payload, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);

            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}

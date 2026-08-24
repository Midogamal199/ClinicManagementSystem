using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ClinicManagementSystem.Infrastructure.ExternalServices
{
    public class PaymobPaymentGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobOptions _options;

        public PaymobPaymentGatewayService(HttpClient httpClient, IOptions<PaymobOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<PaymentGatewayResult> CreateCheckoutSessionAsync(decimal amount, Guid transactionReference)
        {
            try
            {
                var amountInCents = (int)(amount * 100);
                var authToken = await GetAuthTokenAsync();
                var orderId = await RegisterOrderAsync(authToken, amountInCents, transactionReference);
                var paymentKey = await GetPaymentKeyAsync(authToken, orderId, amountInCents);
                var checkoutUrl =
                   $"{_options.BaseUrl}/acceptance/iframes/{_options.IframeId}?payment_token={paymentKey}";
                return new PaymentGatewayResult
                {
                    Success = true,
                    CheckoutUrl = checkoutUrl,
                    GatewayReference = orderId.ToString()
                };
            }

            catch (Exception ex)
            {
                return new PaymentGatewayResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        private async Task<string> GetAuthTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/auth/tokens", new
            {
                api_key = _options.ApiKey
            });
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Paymob auth failed ({(int)response.StatusCode}): {errorBody}");
            }
            var result = await response.Content.ReadFromJsonAsync<PaymobAuthResponse>();
            return result!.Token;
        }
        private async Task<long> RegisterOrderAsync(string authToken, int amountInCents, Guid transactionReference)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/ecommerce/orders");
            request.Headers.Add("Authorization", $"Bearer {authToken}");
            request.Content = JsonContent.Create(new
            {
                auth_token = authToken,
                delivery_needed = false,
                amount_cents = amountInCents,
                currency = _options.Currency,
                merchant_order_id = transactionReference.ToString()
            });
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Paymob order registration failed ({(int)response.StatusCode}): {errorBody}");
            }
            var result = await response.Content.ReadFromJsonAsync<PaymobOrderResponse>();
            return result!.Id;

        }
        private async Task<string> GetPaymentKeyAsync(string authToken, long orderId, int amountInCents)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/acceptance/payment_keys", new
            {
                auth_token = authToken,
                amount_cents = amountInCents,
                expiration = 3600,
                order_id = orderId,
                currency = _options.Currency,
                integration_id = int.Parse(_options.IntegrationId),
                billing_data = new
                {
                    apartment = "NA",
                    email = "customer@example.com",
                    floor = "NA",
                    first_name = "NA",
                    street = "NA",
                    building = "NA",
                    phone_number = "NA",
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "NA",
                    country = "NA",
                    last_name = "NA",
                    state = "NA"
                }
            });
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Paymob payment key request failed ({(int)response.StatusCode}): {errorBody}");
            }
            var result = await response.Content.ReadFromJsonAsync<PaymobPaymentKeyResponse>();
            return result!.Token;
        }
        private class PaymobAuthResponse
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
        }

        private class PaymobOrderResponse
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }
        }

        private class PaymobPaymentKeyResponse
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
        }


    }
}
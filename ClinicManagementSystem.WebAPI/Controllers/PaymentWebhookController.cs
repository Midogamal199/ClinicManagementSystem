using ClinicManagementSystem.Application.Features.Payments.Commands.ConfirmOnlinePayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/webhooks/payment")]
    public class PaymentWebhookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentWebhookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> HandlePaymentWebhook([FromQuery(Name = "hmac")] string signature)
        {
            using var reader = new StreamReader(Request.Body);
            var rawPayload = await reader.ReadToEndAsync();

            using var document = JsonDocument.Parse(rawPayload);
            var obj = document.RootElement.GetProperty("obj");

            var gatewayReference = obj.GetProperty("order").GetProperty("id").GetRawText();
            var isSuccessful = obj.GetProperty("success").GetBoolean();

            var command = new ConfirmOnlinePaymentCommand
            {
                GatewayReference = gatewayReference,
                IsSuccessful = isSuccessful,
                RawPayload = rawPayload,
                Signature = signature
            };

            await _mediator.Send(command);

            return Ok();
        }
    }
}
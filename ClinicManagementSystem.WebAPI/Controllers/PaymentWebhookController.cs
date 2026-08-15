using ClinicManagementSystem.Application.Features.Payments.Commands.ConfirmOnlinePayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> HandlePaymentWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var rawPayload = await reader.ReadToEndAsync();

            var signature = Request.Headers["X-Webhook-Signature"].ToString();

            var payloadData = System.Text.Json.JsonDocument.Parse(rawPayload);
            var gatewayReference = payloadData.RootElement.GetProperty("reference").GetString();
            var isSuccessful = payloadData.RootElement.GetProperty("success").GetBoolean();

            var command = new ConfirmOnlinePaymentCommand
            {
                GatewayReference = gatewayReference!,
                IsSuccessful = isSuccessful,
                RawPayload = rawPayload,
                Signature = signature
            };

            await _mediator.Send(command);

            return Ok();
        }
    }
}
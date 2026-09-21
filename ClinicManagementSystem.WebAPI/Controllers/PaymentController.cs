using ClinicManagementSystem.Application.Features.Payments.Commands.CreatePayment;
using ClinicManagementSystem.Application.Features.Payments.Commands.InitiateOnlinePayment;
using ClinicManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoice;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Receptionist}")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(new { id = paymentId });
        }

        [HttpGet("by-invoice/{invoiceId}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Receptionist},{Roles.Patient}")]

        public async Task<IActionResult> GetByInvoice(Guid invoiceId)
        {
            var payments = await _mediator.Send(new GetPaymentsByInvoiceQuery(invoiceId));
            return Ok(payments);
        }
        [HttpPost("online/initiate")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Receptionist},{Roles.Patient}")]

        public async Task<IActionResult> InitiateOnlinePayment([FromBody] InitiateOnlinePaymentCommand command)
        {
            var checkoutUrl = await _mediator.Send(command);
            return Ok(new { checkoutUrl });
        }
    }
}
using ClinicManagementSystem.Application.Features.Invoices.Commands.CreateInvoice;
using ClinicManagementSystem.Application.Features.Invoices.Queries.GetAllInvoices;
using ClinicManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Receptionist}")]

        public async Task<IActionResult> Create([FromBody] CreateInvoiceCommand command)
        {
            var invoiceId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = invoiceId }, new { id = invoiceId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
            return Ok(invoice);
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Receptionist}")]

        public async Task<IActionResult> GetAll([FromQuery] GetAllInvoicesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
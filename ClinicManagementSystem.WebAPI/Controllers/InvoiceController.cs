using ClinicManagementSystem.Application.Features.Invoices.Commands.CreateInvoice;
using ClinicManagementSystem.Application.Features.Invoices.Queries.GetAllInvoices;
using ClinicManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
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
        public async Task<IActionResult> GetAll([FromQuery] GetAllInvoicesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
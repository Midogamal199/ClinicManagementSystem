using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Reports.Queries.GetMonthlyRevenueReport;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("monthly-revenue/export")]
        public async Task<IActionResult> ExportMonthlyRevenue([FromQuery] GetMonthlyRevenueReportQuery query)
        {
            var file = await _mediator.Send(query);
            return File(file.Content, file.ContentType, file.FileName);
        }
    }
}
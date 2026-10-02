using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDashboardSummary;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDiagnosisTrends;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetFrequentVisitors;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetGenderDistribution;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetHrMetrics;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetRevenueTrend;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetSpecialtyDistribution;
using ClinicManagementSystem.Application.Features.Dashboard.Queries.GetTopVisitedDoctors;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _mediator.Send(new GetDashboardSummaryQuery());
            return Ok(result);
        }

        [HttpGet("diagnosis-trends")]
        public async Task<IActionResult> GetDiagnosisTrends()
        {
            var result = await _mediator.Send(new GetDiagnosisTrendsQuery());
            return Ok(result);
        }

        [HttpGet("revenue-trend")]
        public async Task<IActionResult> GetRevenueTrend()
        {
            var result = await _mediator.Send(new GetRevenueTrendQuery());
            return Ok(result);
        }

        [HttpGet("hr-metrics")]
        public async Task<IActionResult> GetHrMetrics()
        {
            var result = await _mediator.Send(new GetHrMetricsQuery());
            return Ok(result);
        }

        [HttpGet("top-doctors")]
        public async Task<IActionResult> GetTopVisitedDoctors()
        {
            var result = await _mediator.Send(new GetTopVisitedDoctorsQuery());
            return Ok(result);
        }

        [HttpGet("gender-distribution")]
        public async Task<IActionResult> GetGenderDistribution()
        {
            var result = await _mediator.Send(new GetGenderDistributionQuery());
            return Ok(result);
        }

        [HttpGet("specialty-distribution")]
        public async Task<IActionResult> GetSpecialtyDistribution()
        {
            var result = await _mediator.Send(new GetSpecialtyDistributionQuery());
            return Ok(result);
        }

        [HttpGet("frequent-visitors")]
        public async Task<IActionResult> GetFrequentVisitors()
        {
            var result = await _mediator.Send(new GetFrequentVisitorsQuery());
            return Ok(result);
        }
    }
}
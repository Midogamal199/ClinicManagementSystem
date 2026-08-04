using ClinicManagementSystem.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest;
using ClinicManagementSystem.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using ClinicManagementSystem.Application.Features.LeaveRequests.Commands.RejectLeaveRequest;
using ClinicManagementSystem.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;
using ClinicManagementSystem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController: ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command)
        {
            var leaveRequestId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = leaveRequestId }, new { id = leaveRequestId });
        }
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _mediator.Send(new ApproveLeaveRequestCommand(id));
            return NoContent();
        }
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            await _mediator.Send(new RejectLeaveRequestCommand(id));
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var leaveRequest = await _mediator.Send(new GetLeaveRequestByIdQuery(id));
            return Ok(leaveRequest);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllLeaveRequestsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }




    }
}

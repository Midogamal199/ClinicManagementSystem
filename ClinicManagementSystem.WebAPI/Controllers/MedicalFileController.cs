using System;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.MedicalFiles.Queries.GetMedicalFile;
using ClinicManagementSystem.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor},{Roles.Patient}")]
    public class MedicalFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalFileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId)
        {
            var result = await _mediator.Send(new GetMedicalFileQuery(patientId));
            return Ok(result);
        }
    }
}
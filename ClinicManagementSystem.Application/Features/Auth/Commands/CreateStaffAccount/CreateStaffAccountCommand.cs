using System;
using System.Collections.Generic;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.CreateStaffAccount
{
    public class CreateStaffAccountCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid EmployeeId { get; set; }
        public string? LicenseNumber { get; set; }
        public List<Guid> SpecialtyIds { get; set; } = new();
    }
}
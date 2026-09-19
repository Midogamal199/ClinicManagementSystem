using System;
using System.Collections.Generic;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid? PatientId { get; }
        Guid? EmployeeId { get; }
        IReadOnlyList<string> Roles { get; }
        bool IsInRole(string role);
    }
}
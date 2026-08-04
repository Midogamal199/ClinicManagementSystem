using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Attendances;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Queries.GetAllAttendances
{
    public class GetAllAttendancesQuery : IRequest<PaginatedResult<AttendanceDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? EmployeeId { get; set; }
    }
}
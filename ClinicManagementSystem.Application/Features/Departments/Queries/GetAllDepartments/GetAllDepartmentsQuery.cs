using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Departments;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQuery : IRequest<PaginatedResult<DepartmentDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
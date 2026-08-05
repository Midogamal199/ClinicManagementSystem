using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Employees;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQuery: IRequest<PaginatedResult<EmployeeDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? Position { get; set; }
    }
}

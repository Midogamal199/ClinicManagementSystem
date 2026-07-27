using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Employees;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery: IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }

        public GetEmployeeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

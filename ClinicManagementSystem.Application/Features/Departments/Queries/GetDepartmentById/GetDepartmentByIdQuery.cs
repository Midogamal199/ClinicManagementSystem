using ClinicManagementSystem.Application.DTOs.Departments;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQuery : IRequest<DepartmentDto>
    {
        public Guid Id { get; set; }

        public GetDepartmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
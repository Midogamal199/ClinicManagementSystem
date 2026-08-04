using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}
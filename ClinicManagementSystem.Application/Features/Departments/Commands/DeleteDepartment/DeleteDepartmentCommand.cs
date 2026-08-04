using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
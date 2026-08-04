using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Commands.CheckOut
{
    public class CheckOutCommand : IRequest<Unit>
    {
        public Guid EmployeeId { get; set; }
    }
}
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Queries.GetRemainingLeaveDays
{
    public class GetRemainingLeaveDaysQuery : IRequest<int>
    {
        public Guid EmployeeId { get; set; }

        public GetRemainingLeaveDaysQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
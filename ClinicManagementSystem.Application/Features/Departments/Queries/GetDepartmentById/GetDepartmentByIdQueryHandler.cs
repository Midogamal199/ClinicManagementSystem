using ClinicManagementSystem.Application.DTOs.Departments;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentDto> Handle(
            GetDepartmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var (department, employeeCount) = await _unitOfWork.DepartmentRepository
                .GetByIdWithEmployeeCountAsync(request.Id);

            if (department is null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.Id}' was not found.");
            }

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                EmployeeCount = employeeCount
            };
        }
    }
}
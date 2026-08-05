using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.DepartmentId);

            if (department is null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.DepartmentId}' was not found.");
            }
            var employee = new Employee
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Position = request.Position,
                Salary = request.Salary,
                DepartmentId = request.DepartmentId
            };
            await _unitOfWork.Repository<Employee>().AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return employee.Id;

        }
    }
}

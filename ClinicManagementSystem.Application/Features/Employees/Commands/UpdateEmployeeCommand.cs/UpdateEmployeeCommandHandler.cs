using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Commands.UpdateEmployeeCommand.cs
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(request.Id);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' was not found.");
            }

            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.DepartmentId);

            if (department is null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.DepartmentId}' was not found.");
            }
            employee.FullName = request.FullName;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Salary = request.Salary;
            employee.DepartmentId = request.DepartmentId;

            _unitOfWork.Repository<Employee>().Update(employee);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(request.Id);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' was not found.");
            }
            var HasLinkedDoctor = await _unitOfWork.EmployeeRepository.HasLinkedDoctorAsync(request.Id);

            if (HasLinkedDoctor)
            {
                throw new InvalidOperationException(
                    "Cannot delete an employee who is registered as a doctor. Delete the doctor record first.");
            }
            _unitOfWork.Repository<Employee>().Delete(employee);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

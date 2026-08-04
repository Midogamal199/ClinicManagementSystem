using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.Id);

            if (department is null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.Id}' was not found.");
            }

            var hasEmployees = await _unitOfWork.DepartmentRepository.HasEmployeesAsync(request.Id);

            if (hasEmployees)
            {
                throw new InvalidOperationException(
                    "Cannot delete a department that still has employees assigned to it.");
            }

            _unitOfWork.Repository<Department>().Delete(department);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
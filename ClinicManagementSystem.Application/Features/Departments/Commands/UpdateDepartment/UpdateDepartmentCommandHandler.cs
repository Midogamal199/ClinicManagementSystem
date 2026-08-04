using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.Id);

            if (department is null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.Id}' was not found.");
            }

            department.Name = request.Name;

            _unitOfWork.Repository<Department>().Update(department);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
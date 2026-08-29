using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Features.Doctors.Commands.CreateDoctor;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.CreateStaffAccount
{
    public class CreateStaffAccountCommandHandler : IRequestHandler<CreateStaffAccountCommand, string>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateStaffAccountCommandHandler(
            IIdentityService identityService,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<string> Handle(CreateStaffAccountCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _identityService.UserExistsAsync(request.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");
            }
            var employee = await _unitOfWork.EmployeeRepository.GetByIdWithDetailsAsync(request.EmployeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' was not found.");
            }
            var employeeHasAccount = await _identityService.EmployeeHasAccountAsync(request.EmployeeId);
            if (employeeHasAccount)
            {
                throw new InvalidOperationException("This employee already has a registered account.");
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (request.Role == "Doctor")
                {
                    var hasLinkedDoctor = await _unitOfWork.EmployeeRepository.HasLinkedDoctorAsync(request.EmployeeId);
                    if (!hasLinkedDoctor)
                    {
                        await _mediator.Send(new CreateDoctorCommand
                        {
                            EmployeeId = request.EmployeeId,
                            LicenseNumber = request.LicenseNumber!,
                            SpecialtyIds = request.SpecialtyIds
                        }, cancellationToken);
                    }
                }

                var result = await _identityService.CreateStaffAccountAsync(
                    request.Email, request.Password, request.Role, request.EmployeeId);

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException($"Failed to create staff account: {string.Join("; ", result.Errors)}");
                }

                await _unitOfWork.CommitTransactionAsync();
                return result.UserId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

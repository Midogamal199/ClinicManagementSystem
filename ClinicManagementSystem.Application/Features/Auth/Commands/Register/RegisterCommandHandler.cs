using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _identityService.UserExistsAsync(request.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");
            }

            var normalizedFullName = request.FullName.Trim().ToLower();

            var matches = await _unitOfWork.Repository<Patient>().FindAsync(
                p => p.FullName.Trim().ToLower() == normalizedFullName
                  && p.PhoneNumber == request.PhoneNumber
                  && p.DateOfBirth == request.DateOfBirth);

            var existingPatient = matches.FirstOrDefault();

            if (existingPatient is not null)
            {
                var patientHasAccount = await _identityService.PatientHasAccountAsync(existingPatient.Id);
                if (patientHasAccount)
                {
                    throw new InvalidOperationException("An account already exists for this patient.");
                }

                var linkResult = await _identityService.RegisterPatientAsync(request.Email, request.Password, existingPatient.Id);
                if (!linkResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user: {string.Join("; ", linkResult.Errors)}");
                }

                return linkResult.UserId;
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var newPatient = new Patient
                {
                    FullName = request.FullName,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                };

                await _unitOfWork.Repository<Patient>().AddAsync(newPatient);
                await _unitOfWork.SaveChangesAsync();

                var createResult = await _identityService.RegisterPatientAsync(request.Email, request.Password, newPatient.Id);
                if (!createResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException($"Failed to create user: {string.Join("; ", createResult.Errors)}");
                }

                await _unitOfWork.CommitTransactionAsync();
                return createResult.UserId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
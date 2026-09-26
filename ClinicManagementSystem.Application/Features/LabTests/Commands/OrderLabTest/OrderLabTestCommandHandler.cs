using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.LabTests.Commands.OrderLabTest
{
    public class OrderLabTestCommandHandler : IRequestHandler<OrderLabTestCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public OrderLabTestCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(OrderLabTestCommand request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.VisitRepository.GetByIdWithDetailsAsync(request.VisitId);
            if (visit is null)
            {
                throw new KeyNotFoundException($"Visit with Id '{request.VisitId}' was not found.");
            }
            if (_currentUserService.IsInRole("Doctor") &&
                            visit.Appointment.DoctorId != _currentUserService.DoctorId)
            {
                throw new ForbiddenAccessException("You can only order lab tests for your own visits.");
            }
            var labTest = new LabTest
            {
                VisitId = request.VisitId,
                TestType = request.TestType,
                Status = LabTestStatus.Pending
            };
            await _unitOfWork.Repository<LabTest>().AddAsync(labTest);
            await _unitOfWork.SaveChangesAsync();
            return labTest.Id;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Visits.Commands.CreateVisit
{
    public class CreateVisitCommandHandler : IRequestHandler<CreateVisitCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateVisitCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateVisitCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(request.AppointmentId);
            if (appointment is null)
            {
                throw new KeyNotFoundException($"Appointment with Id '{request.AppointmentId}' was not found.");
            }
            var existingVisits = await _unitOfWork.Repository<Visit>().FindAsync(v => v.AppointmentId == request.AppointmentId);
            if (existingVisits.Any())
            {
                throw new InvalidOperationException(
                    $"Appointment '{request.AppointmentId}' already has a visit recorded.");
            }
            var visit=new Visit
            {
                AppointmentId = request.AppointmentId,
                VisitDate = request.VisitDate,
                Notes = request.Notes
            };
            await _unitOfWork.Repository<Visit>().AddAsync(visit);
            appointment.Status = AppointmentStatus.Completed;
            _unitOfWork.Repository<Appointment>().Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return visit.Id;
        }
    }
}

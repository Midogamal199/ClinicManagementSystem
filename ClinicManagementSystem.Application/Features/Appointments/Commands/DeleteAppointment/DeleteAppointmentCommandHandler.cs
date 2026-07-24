using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAppointmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(request.Id);

            if (appointment is null)
            {
                throw new KeyNotFoundException($"Appointment with Id '{request.Id}' was not found.");
            }
            var linkedVisits= await _unitOfWork.Repository<Visit>().FindAsync(v=> v.AppointmentId == request.Id);
            if (linkedVisits.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete an appointment that already has a recorded visit.");
            }
            _unitOfWork.Repository<Appointment>().Delete(appointment);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        
    }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Enums;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DateTime ScheduledAt { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}

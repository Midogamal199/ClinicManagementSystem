using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand: IRequest<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}

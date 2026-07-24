using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommandValidator: AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Appointment Id is required.");

            RuleFor(x => x.ScheduledAt)
                .NotEmpty().WithMessage("Scheduled date is required.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}

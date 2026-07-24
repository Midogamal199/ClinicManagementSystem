using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandValidator : AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Appointment Id is required.");
        }
    }
}
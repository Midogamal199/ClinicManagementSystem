using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient Id is required.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor Id is required.");

            RuleFor(x => x.ScheduledAt)
                .NotEmpty().WithMessage("Scheduled date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future.");
        }
    }
}
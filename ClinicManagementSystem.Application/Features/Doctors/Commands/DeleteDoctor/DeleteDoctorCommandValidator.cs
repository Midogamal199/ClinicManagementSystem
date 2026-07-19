using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Doctors.Commands.DeleteDoctor
{
    public class DeleteDoctorCommandValidator: AbstractValidator<DeleteDoctorCommand>
    {
        public DeleteDoctorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Doctor Id is required.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Enums;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}

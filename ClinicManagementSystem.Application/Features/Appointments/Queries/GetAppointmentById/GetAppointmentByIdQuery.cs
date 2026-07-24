using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Appointments;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQuery: IRequest<AppointmentDto>
    {
        public Guid Id { get; set; }

        public GetAppointmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Appointments;
using ClinicManagementSystem.Domain.Enums;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Appointments.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQuery: IRequest<PaginatedResult<AppointmentDto>>
    {

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? DoctorId { get; set; }
        public Guid? PatientId { get; set; }
        public AppointmentStatus? Status { get; set; }
    }
}

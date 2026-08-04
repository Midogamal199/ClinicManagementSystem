using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Attendances;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Queries.GetAttendanceById
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceDto>
    {
        public Guid Id { get; set; }

        public GetAttendanceByIdQuery(Guid id)
        {
            Id = id;
    }   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Queries.GetWorkingHours
{
    public class GetWorkingHoursQuery: IRequest<double>
    {
        public Guid AttendanceId { get; set; }

        public GetWorkingHoursQuery(Guid attendanceId)
        {
            AttendanceId = attendanceId;
        }
    }
}

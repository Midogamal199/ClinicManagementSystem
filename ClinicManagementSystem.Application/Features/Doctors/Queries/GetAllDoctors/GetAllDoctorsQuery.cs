using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Doctors;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Queries.GetAllDoctors
{
    public class GetAllDoctorsQuery: IRequest<PaginatedResult<DoctorDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Doctors;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Doctors.Queries.GetDoctorById
{
    public class GetDoctorByIdQuery:IRequest<DoctorDto>
    {
        public Guid Id { get; set; }
        public GetDoctorByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}

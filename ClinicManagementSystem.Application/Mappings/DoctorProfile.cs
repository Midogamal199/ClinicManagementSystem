using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Doctors;
using ClinicManagementSystem.Domain.Entities;
using Microsoft.Extensions.Options;

namespace ClinicManagementSystem.Application.Mappings
{
    public class DoctorProfile: Profile
    {
        public DoctorProfile() {
            CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee.FullName))
            .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties.Select(s => s.Name)));




        }
    }
}

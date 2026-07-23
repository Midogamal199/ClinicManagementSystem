using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Visits;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class VisitProfile:Profile
    {
        public VisitProfile() { 
        CreateMap<Visit,VisitDto>()
        .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src =>src.Appointment.Patient.FullName))
        .ForMember(dest => dest.DoctorLicenseNumber, opt=>opt.MapFrom(src => src.Appointment.Doctor.LicenseNumber))
        .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Appointment.Doctor.Employee.FullName));




        }
    }
}

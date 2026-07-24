using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Appointments;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class AppointmentProfile:Profile    
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientFullName,
                    opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.DoctorFullName,
                    opt => opt.MapFrom(src => src.Doctor.Employee.FullName));
        }
    }
}

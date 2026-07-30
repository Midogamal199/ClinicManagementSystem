using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class PrescriptionProfile:Profile
    {
        public PrescriptionProfile()
        {
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.PatientFullName,
                    opt => opt.MapFrom(src => src.Visit.Appointment.Patient.FullName));

            CreateMap<PrescriptionItem, PrescriptionItemDto>();
        }
    }
}

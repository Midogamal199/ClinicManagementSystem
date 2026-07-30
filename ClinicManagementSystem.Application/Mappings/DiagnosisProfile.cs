using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Diagnoses;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class DiagnosisProfile : Profile
    {
        public DiagnosisProfile()
        {
            CreateMap<Diagnosis, DiagnosisDto>()
                .ForMember(dest => dest.PatientFullName,
                    opt => opt.MapFrom(src => src.Visit.Appointment.Patient.FullName));
        }
    }
}
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Specialties;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class SpecialtyProfile : Profile
    {
        public SpecialtyProfile()
        {
            CreateMap<Specialty, SpecialtyDto>()
                .ForMember(dest => dest.DoctorCount, opt => opt.MapFrom(src => src.Doctors.Count));

            CreateMap<Doctor, DoctorSummaryDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Employee.FullName));
        }
    }
}
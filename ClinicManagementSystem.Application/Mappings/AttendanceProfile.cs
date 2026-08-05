using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Attendances;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<Attendance, AttendanceDto>()
                .ForMember(dest => dest.EmployeeFullName,
                    opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.WorkingHours,
                    opt => opt.MapFrom(src => src.CheckOut.HasValue
                   ? Math.Round((src.CheckOut.Value - src.CheckIn).TotalHours, 2)
                    : (double?)null));
 
        }
    }
}
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.LeaveRequests;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Mappings
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(dest => dest.EmployeeFullName,
                    opt => opt.MapFrom(src => src.Employee.FullName));
        }
    }
}
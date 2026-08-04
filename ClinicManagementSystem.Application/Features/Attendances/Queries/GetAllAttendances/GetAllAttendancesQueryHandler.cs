using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Attendances;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Attendances.Queries.GetAllAttendances
{
    public class GetAllAttendancesQueryHandler
        : IRequestHandler<GetAllAttendancesQuery, PaginatedResult<AttendanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAttendancesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<AttendanceDto>> Handle(
            GetAllAttendancesQuery request,
            CancellationToken cancellationToken)
        {
            var (attendances, totalCount) = await _unitOfWork.AttendanceRepository.GetPagedWithDetailsAsync(
               request.PageNumber,
               request.PageSize,
               request.EmployeeId);
            var dtos = _mapper.Map<List<AttendanceDto>>(attendances);
            return new PaginatedResult<AttendanceDto>
            {
                Items = dtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}

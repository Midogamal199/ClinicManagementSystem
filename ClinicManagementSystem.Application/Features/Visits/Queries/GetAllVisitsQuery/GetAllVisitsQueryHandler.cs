using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Visits;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Visits.Queries.GetAllVisitsQuery
{
    public class GetAllVisitsQueryHandler : IRequestHandler<GetAllVisitsQuery, PaginatedResult<VisitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVisitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<VisitDto>> Handle(GetAllVisitsQuery request, CancellationToken cancellationToken)
        {
            var (visits , totalCount) = await _unitOfWork.VisitRepository.GetPagedWithDetailsAsync(
                request.PageNumber,
                request.PageSize);
            var dtos = _mapper.Map<List<VisitDto>>(visits);

            return new PaginatedResult<VisitDto>
            {
                Items = dtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

        }
    }
}

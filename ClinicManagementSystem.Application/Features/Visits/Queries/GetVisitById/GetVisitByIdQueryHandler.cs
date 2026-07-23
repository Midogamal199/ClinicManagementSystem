using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Visits;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Visits.Queries.GetVisitById
{
    public class GetVisitByIdQueryHandler : IRequestHandler<GetVisitByIdQuery, VisitDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVisitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<VisitDto> Handle(GetVisitByIdQuery request, CancellationToken cancellationToken)
        {
            var visit= await _unitOfWork.VisitRepository.GetByIdWithDetailsAsync(request.Id);
            if (visit is null)
            {
                throw new KeyNotFoundException($"Visit with Id '{request.Id}' was not found.");
            }
            return _mapper.Map<VisitDto>(visit);
        }
    }
}

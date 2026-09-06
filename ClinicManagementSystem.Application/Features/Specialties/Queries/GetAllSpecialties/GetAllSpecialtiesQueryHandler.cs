using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Specialties;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Queries.GetAllSpecialties
{
    public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, List<SpecialtyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSpecialtiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<SpecialtyDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
        {
            var specialties = await _unitOfWork.SpecialtyRepository.GetAllWithDoctorsAsync();
            return _mapper.Map<List<SpecialtyDto>>(specialties);
        }
    }
}

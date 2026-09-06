using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Specialties;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Queries.GetSpecialtyById
{
    public class GetSpecialtyByIdQueryHandler : IRequestHandler<GetSpecialtyByIdQuery, SpecialtyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecialtyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SpecialtyDto> Handle(GetSpecialtyByIdQuery request, CancellationToken cancellationToken)
        {
            var specialty = await _unitOfWork.SpecialtyRepository.GetByIdWithDoctorsAsync(request.Id);
            if (specialty is null)
            {
                throw new KeyNotFoundException($"Specialty with Id '{request.Id}' was not found.");
            }
            return _mapper.Map<SpecialtyDto>(specialty);


        }
    }
}

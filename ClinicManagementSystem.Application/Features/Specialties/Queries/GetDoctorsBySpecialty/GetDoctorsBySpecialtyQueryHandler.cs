using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Specialties;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Specialties.Queries.GetDoctorsBySpecialty
{
    public class GetDoctorsBySpecialtyQueryHandler : IRequestHandler<GetDoctorsBySpecialtyQuery, List<DoctorSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDoctorsBySpecialtyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<DoctorSummaryDto>> Handle(GetDoctorsBySpecialtyQuery request, CancellationToken cancellationToken)
        {
            var specialty = await _unitOfWork.SpecialtyRepository.GetByIdWithDoctorsAsync(request.SpecialtyId);
            if (specialty is null)
            {
                throw new KeyNotFoundException($"Specialty with Id '{request.SpecialtyId}' was not found.");
            }
            return _mapper.Map<List<DoctorSummaryDto>>(specialty.Doctors);

        }
    }
}

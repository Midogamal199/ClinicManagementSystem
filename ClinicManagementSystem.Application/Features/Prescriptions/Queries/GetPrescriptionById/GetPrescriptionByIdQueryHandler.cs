using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById
{
    public class GetPrescriptionByIdQueryHandler : IRequestHandler<GetPrescriptionByIdQuery, PrescriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPrescriptionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PrescriptionDto> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var prescription = await _unitOfWork.PrescriptionRepository.GetByIdWithDetailsAsync(request.Id);
            if (prescription is null)
            {
                throw new KeyNotFoundException($"Prescription with Id '{request.Id}' was not found.");
            }
            return _mapper.Map<PrescriptionDto>(prescription);
        }
    }
}

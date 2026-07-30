using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Diagnoses;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Queries.GetDiagnosisById
{
    public class GetDiagnosisByIdQueryHandler : IRequestHandler<GetDiagnosisByIdQuery, DiagnosisDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDiagnosisByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiagnosisDto> Handle(GetDiagnosisByIdQuery request, CancellationToken cancellationToken)
        {
            var diagnosis = await _unitOfWork.DiagnosisRepository.GetByIdWithDetailsAsync(request.Id);
            if (diagnosis is null)
            {
                throw new KeyNotFoundException($"Diagnosis with Id '{request.Id}' was not found.");
            }
            return _mapper.Map<DiagnosisDto>(diagnosis);
        }
    }
}

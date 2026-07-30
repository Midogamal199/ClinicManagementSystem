using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Diagnoses;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Diagnoses.Queries.GetDiagnosesByVisit
{
    public class GetDiagnosesByVisitQueryHandler : IRequestHandler<GetDiagnosesByVisitQuery, List<DiagnosisDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDiagnosesByVisitQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<DiagnosisDto>> Handle(GetDiagnosesByVisitQuery request, CancellationToken cancellationToken)
        {
            var diagnoses = await _unitOfWork.DiagnosisRepository.GetByVisitIdAsync(request.VisitId);
            return _mapper.Map<List<DiagnosisDto>>(diagnoses);
        }
    }
}

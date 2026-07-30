using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Prescriptions;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionsByVisit
{
    public class GetPrescriptionsByVisitQueryHandler
        : IRequestHandler<GetPrescriptionsByVisitQuery, List<PrescriptionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPrescriptionsByVisitQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PrescriptionDto>> Handle(
            GetPrescriptionsByVisitQuery request,
            CancellationToken cancellationToken)
        {
            var prescriptions = await _unitOfWork.PrescriptionRepository.GetByVisitIdAsync(request.VisitId);
            return _mapper.Map<List<PrescriptionDto>>(prescriptions);
        }
    }
}
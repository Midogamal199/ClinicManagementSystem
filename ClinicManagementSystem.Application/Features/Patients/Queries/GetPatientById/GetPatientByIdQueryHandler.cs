using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.DTOs.Patients;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Queries.GetPatientById
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto>

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetPatientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(request.Id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Patient with Id '{request.Id}' was not found.");
            }
            return _mapper.Map<PatientDto>(patient);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreatePatientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
           var patient = _mapper.Map<Patient>(request);
            await _unitOfWork.Repository<Patient>().AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
            return patient.Id;
        }
    }
}

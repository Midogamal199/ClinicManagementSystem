using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Patients;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Patients.Queries.GetAllPatients
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, PaginatedResult<PatientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllPatientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var (patients, totalCount) = await _unitOfWork.Repository<Patient>().GetPagedAsync(
                         request.PageNumber,
                         request.PageSize,
                         filter: string.IsNullOrWhiteSpace(request.SearchTerm)
                             ? null
                             : p => p.FullName.Contains(request.SearchTerm));
            var dtos = _mapper.Map<List<PatientDto>>(patients);
            return new PaginatedResult<PatientDto>
            {
                Items = dtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
            }
    }
}

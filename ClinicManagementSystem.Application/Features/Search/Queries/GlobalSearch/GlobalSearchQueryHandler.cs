using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common.Exceptions;
using ClinicManagementSystem.Application.DTOs.Search;
using ClinicManagementSystem.Application.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Search.Queries.GlobalSearch
{
    public class GlobalSearchQueryHandler : IRequestHandler<GlobalSearchQuery, GlobalSearchResultDto>
    {
        private readonly ISearchRepository _searchRepository;
        private readonly ICurrentUserService _currentUserService;

        public GlobalSearchQueryHandler(
            ISearchRepository searchRepository,
            ICurrentUserService currentUserService)
        {
            _searchRepository = searchRepository;
            _currentUserService = currentUserService;
        }
        public async Task<GlobalSearchResultDto> Handle(GlobalSearchQuery request, CancellationToken cancellationToken)
        {
            var term=request.Term.Trim();
            var limit=request.Limit;
            var isAdmin = _currentUserService.IsInRole("Admin");
            var isDoctor = _currentUserService.IsInRole("Doctor");
            var isReceptionist = _currentUserService.IsInRole("Receptionist");
            if (!isAdmin && !isDoctor && !isReceptionist)
            {
                throw new ForbiddenAccessException("You are not allowed to use the search.");
            }
            Guid? doctorScope = null;
            if (!isAdmin && isDoctor)
            {
                doctorScope = _currentUserService.DoctorId
                    ?? throw new ForbiddenAccessException("No doctor profile is linked to this account.");
            }
            var result = new GlobalSearchResultDto { Term = term };
            var includeDiagnosisMatches= isAdmin || isDoctor;
            result.Patients = await _searchRepository.SearchPatientsAsync(
                term, limit, includeDiagnosisMatches, doctorScope);
            if (isAdmin)
            {
                result.Employees = await _searchRepository.SearchEmployeesAsync(term, limit);
            }
            result.Prescriptions = await _searchRepository.SearchPrescriptionsAsync(term, limit, doctorScope);
            result.Reports = await _searchRepository.SearchLabTestsAsync(term, limit, doctorScope);
            return result;


        }
    }
}

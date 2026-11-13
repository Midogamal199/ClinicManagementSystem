using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Search;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface ISearchRepository
    {
        Task<List<SearchHitDto>> SearchPatientsAsync(
            string term, int limit, bool includeDiagnosisMatches, Guid? restrictDiagnosesToDoctorId);

        Task<List<SearchHitDto>> SearchEmployeesAsync(string term, int limit);

        Task<List<SearchHitDto>> SearchPrescriptionsAsync(string term, int limit, Guid? doctorId);

        Task<List<SearchHitDto>> SearchLabTestsAsync(string term, int limit, Guid? doctorId);

    }
}

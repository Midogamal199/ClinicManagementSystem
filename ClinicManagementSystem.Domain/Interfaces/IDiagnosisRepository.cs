using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IDiagnosisRepository: IGenericRepository<Diagnosis>
    {
        Task<Diagnosis?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Diagnosis>> GetByVisitIdAsync(Guid visitId);
    }
}

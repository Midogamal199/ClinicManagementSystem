using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IPrescriptionRepository: IGenericRepository<Prescription>
    {
        Task<Prescription?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Prescription>> GetByVisitIdAsync(Guid visitId);
    }
}

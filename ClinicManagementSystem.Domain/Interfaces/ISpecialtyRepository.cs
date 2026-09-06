using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface ISpecialtyRepository : IGenericRepository<Specialty>
    {
        Task<Specialty?> GetByIdWithDoctorsAsync(Guid id);
        Task<IEnumerable<Specialty>> GetAllWithDoctorsAsync();
        Task<bool> HasLinkedDoctorsAsync(Guid specialtyId);
    }
}
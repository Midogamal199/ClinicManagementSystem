using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IDoctorRepository:IGenericRepository<Doctor>
    {
        Task<Doctor?> GetByIdWithSpecialtiesAsync(Guid id);

    }
}

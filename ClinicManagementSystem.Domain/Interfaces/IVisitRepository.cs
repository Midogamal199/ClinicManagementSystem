using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
   public interface IVisitRepository:IGenericRepository<Visit>
    {
        Task<Visit?> GetByIdWithDetailsAsync(Guid id);
        Task<(IEnumerable<Visit> Items, int TotalCount)> GetPagedWithDetailsAsync(
         int pageNumber, int pageSize);


    }
}

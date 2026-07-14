using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Interfaces
{
   public interface IGenericRepository<T> where T :BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
  
        Task <IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<(IEnumerable<T> items,int Totalcount)> GetPagedAsync(int pageNumber,
            int pageSize,    Expression<Func<T, bool>>? filter = null);

        


    }
}

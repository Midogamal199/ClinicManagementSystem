using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Common;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        IDoctorRepository DoctorRepository { get; }
        IVisitRepository VisitRepository { get; }
        Task<int> SaveChangesAsync();

    }
}

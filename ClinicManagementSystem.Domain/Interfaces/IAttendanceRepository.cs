using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IAttendanceRepository:IGenericRepository<Attendance>
    {
        Task<Attendance?> GetByIdWithDetailsAsync(Guid id);

        Task<(IEnumerable<Attendance> Items, int TotalCount)> GetPagedWithDetailsAsync(
            int pageNumber,
            int pageSize,
            Guid? employeeId);

        Task<Attendance?> GetOpenAttendanceForEmployeeAsync(Guid employeeId);
    }
}

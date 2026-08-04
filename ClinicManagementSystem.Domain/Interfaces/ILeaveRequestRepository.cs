using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface ILeaveRequestRepository:IGenericRepository<LeaveRequest>
    {
        Task<LeaveRequest?> GetByIdWithDetailsAsync(Guid id);
        Task<(IEnumerable<LeaveRequest> Items, int TotalCount)> GetPagedWithDetailsAsync(
          int pageNumber,
          int pageSize,
          Guid? employeeId,
          LeaveStatus? status);
        Task<bool> HasOverlapAsync(Guid employeeId, DateTime startDate, DateTime endDate);

    }
}

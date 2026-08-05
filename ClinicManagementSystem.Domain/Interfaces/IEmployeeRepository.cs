using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
  public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        Task<Employee?> GetByIdWithDetailsAsync(Guid id);
        Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedWithDetailsAsync(
          int pageNumber,
          int pageSize,
          string? searchTerm,
          Guid? departmentId,
             string? position);
        Task<bool> HasLinkedDoctorAsync(Guid employeeId);
    }
}

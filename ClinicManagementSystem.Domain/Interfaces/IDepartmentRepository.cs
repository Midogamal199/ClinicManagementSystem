using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<(Department? Department, int EmployeeCount)> GetByIdWithEmployeeCountAsync(Guid id);

       
        Task<(IEnumerable<(Department Department, int EmployeeCount)> Items, int TotalCount)> GetPagedWithEmployeeCountAsync(
            int pageNumber, int pageSize);

        Task<bool> HasEmployeesAsync(Guid departmentId);
    }
}
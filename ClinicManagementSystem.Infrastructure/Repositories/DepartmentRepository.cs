using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(Department? Department, int EmployeeCount)> GetByIdWithEmployeeCountAsync(Guid id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department is null)
            {
                return (null, 0);
            }

            var employeeCount = await _context.Employees.CountAsync(e => e.DepartmentId == id);

            return (department, employeeCount);
        }

       
        public async Task<(IEnumerable<(Department Department, int EmployeeCount)> Items, int TotalCount)> GetPagedWithEmployeeCountAsync(
            int pageNumber, int pageSize)
        {
            var totalCount = await _context.Departments.CountAsync();

            var departmentsData = await _context.Departments
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new
                {
                    Department = d,
                    EmployeeCount = d.Employees.Count()
                })
                .ToListAsync();

            var items = departmentsData.Select(x => (x.Department, x.EmployeeCount));

            return (items, totalCount);
        }

        public async Task<bool> HasEmployeesAsync(Guid departmentId)
        {
            return await _context.Employees.AnyAsync(e => e.DepartmentId == departmentId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e=> e.Id == id);
        }

        public async Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize, string? searchTerm, Guid? departmentId, string? position)
        {
            IQueryable<Employee> query = _context.Employees.Include(e => e.Department);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.FullName.Contains(searchTerm));
            }
            if(departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }
            if (!string.IsNullOrWhiteSpace(position))
            {
                query = query.Where(e => e.Position.Contains(position));
            }
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);

        }

        public async Task<bool> HasLinkedDoctorAsync(Guid employeeId)
        {
            return await _context.Doctors.AnyAsync(d => d.EmployeeId == employeeId);
        }
    }
}

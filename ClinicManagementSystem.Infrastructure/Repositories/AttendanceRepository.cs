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
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Attendance?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Attendances
                 .Include(a => a.Employee)
                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attendance?> GetOpenAttendanceForEmployeeAsync(Guid employeeId)
        {
            return await _context.Attendances.Where(a => a.EmployeeId == employeeId && a.CheckOut == null).OrderByDescending(a => a.CheckIn).FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Attendance> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize, Guid? employeeId)
        {
            IQueryable<Attendance> query = _context.Attendances
                .Include(a => a.Employee);

            if (employeeId.HasValue)
            {
                query = query.Where(a => a.EmployeeId == employeeId.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CheckIn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

    }
}

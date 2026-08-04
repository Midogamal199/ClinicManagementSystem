using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Enums;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LeaveRequest?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.LeaveRequests.Include(l=>l.Employee).FirstOrDefaultAsync(l=>l.Id==id);
        }

        public async Task<(IEnumerable<LeaveRequest> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize, Guid? employeeId, LeaveStatus? status)
        {
            IQueryable<LeaveRequest> query = _context.LeaveRequests
               .Include(l => l.Employee);

            if (employeeId.HasValue)
            {
                query = query.Where(l => l.EmployeeId == employeeId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(l => l.Status == status.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
               .OrderByDescending(l => l.StartDate)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> HasOverlapAsync(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.LeaveRequests.AnyAsync(l =>
                 l.EmployeeId == employeeId &&
                 l.Status != LeaveStatus.Rejected &&
                 l.StartDate <= endDate &&
                 l.EndDate >= startDate);
        }
    }
}

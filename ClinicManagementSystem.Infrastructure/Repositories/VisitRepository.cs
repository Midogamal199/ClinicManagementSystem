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
    public class VisitRepository : GenericRepository<Visit>, IVisitRepository
    {
        private readonly ApplicationDbContext _context;
        public VisitRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Visit?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Visits
                .Include(v => v.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(v => v.Appointment)
                .ThenInclude(a => a.Doctor)
                .ThenInclude(d => d.Employee)
                .FirstOrDefaultAsync(v => v.Id == id);
            
        }

        public async Task<(IEnumerable<Visit> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize)
        {
            IQueryable<Visit> query = _context.Visits
    .Include(v => v.Appointment)
        .ThenInclude(a => a.Patient)
    .Include(v => v.Appointment)
        .ThenInclude(a => a.Doctor)
            .ThenInclude(d => d.Employee);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(v => v.VisitDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);

        }
    }
}

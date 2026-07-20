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
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Doctors
                .Include(d => d.Specialties)
                .Include(d=>d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetByIdWithSpecialtiesAsync(Guid id)
        {
            return await _context.Doctors
                .Include(d => d.Specialties)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<(IEnumerable<Doctor> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            IQueryable<Doctor> query = _context.Doctors
                .Include(d => d.Specialties)
                .Include(d => d.Employee);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.Employee.FullName.Contains(searchTerm) || d.Specialties.Any(s => s.Name.Contains(searchTerm)));
            }
            var totalCount = await query.CountAsync();
            var items = await query
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();

            return (items, totalCount);
        }
    }
}

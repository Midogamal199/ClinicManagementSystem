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
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecialtyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Specialty>> GetAllWithDoctorsAsync()
        {
            return await _context.Specialties
                .Include(s => s.Doctors)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Specialty?> GetByIdWithDoctorsAsync(Guid id)
        {
            return await _context.Specialties
               .Include(s => s.Doctors)
                   .ThenInclude(d => d.Employee)
               .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> HasLinkedDoctorsAsync(Guid specialtyId)
        {
            return await _context.Specialties
                           .Where(s => s.Id == specialtyId)
                           .SelectMany(s => s.Doctors)
                           .AnyAsync();
        }
    }
}

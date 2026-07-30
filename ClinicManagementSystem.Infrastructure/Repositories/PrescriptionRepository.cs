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
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Prescription?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Prescriptions
               .Include(p => p.Items)
               .Include(p => p.Visit)
                   .ThenInclude(v => v.Appointment)
                       .ThenInclude(a => a.Patient)
               .FirstOrDefaultAsync(p => p.Id == id);


        }

        public async Task<IEnumerable<Prescription>> GetByVisitIdAsync(Guid visitId)
        {
            return await _context.Prescriptions
               .Include(p => p.Items)
               .Include(p => p.Visit)
                   .ThenInclude(v => v.Appointment)
                       .ThenInclude(a => a.Patient)
               .Where(p => p.VisitId == visitId)
               .ToListAsync();
        }
    }
}

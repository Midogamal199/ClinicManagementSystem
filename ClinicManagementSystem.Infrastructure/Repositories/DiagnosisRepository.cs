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
    public class DiagnosisRepository : GenericRepository<Diagnosis>, IDiagnosisRepository
    {

        private readonly ApplicationDbContext _context;

        public DiagnosisRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Diagnosis?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Diagnoses
                  .Include(d => d.Visit)
                      .ThenInclude(v => v.Appointment)
                          .ThenInclude(a => a.Patient)
                  .FirstOrDefaultAsync(d => d.Id == id);

        }

        public async Task<IEnumerable<Diagnosis>> GetByVisitIdAsync(Guid visitId)
        {
            return await _context.Diagnoses
               .Include(d => d.Visit)
                   .ThenInclude(v => v.Appointment)
                       .ThenInclude(a => a.Patient)
               .Where(d => d.VisitId == visitId)
               .ToListAsync();
        }
    }
}

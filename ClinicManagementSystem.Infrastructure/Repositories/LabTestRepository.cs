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
    class LabTestRepository : GenericRepository<LabTest>, ILabTestRepository
    {
        private readonly ApplicationDbContext _context;

        public LabTestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LabTest?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.LabTests
               .Include(l => l.Visit)
                   .ThenInclude(v => v.Appointment)
               .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}

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
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Invoice?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.
                Invoices
                .Include(i => i.Patient)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize, Guid? patientId, InvoiceStatus? status)
        {
            IQueryable<Invoice> query = _context.Invoices
              .Include(i => i.Patient)
              .Include(i => i.Payments);
            if (patientId.HasValue)
            {
                query = query.Where(i => i.PatientId == patientId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}

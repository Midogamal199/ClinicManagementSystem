using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Reports;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<RevenuePaymentRowDto>> GetPaymentsInRangeAsync(DateTime fromInclusive, DateTime toExclusive)
        {
            return await _context.Payments
                .IgnoreQueryFilters()
                .Where(p => !p.IsDeleted
                && p.CreatedAt >= fromInclusive
                && p.CreatedAt < toExclusive
                )
                .OrderBy(p => p.CreatedAt)
                .Select(p=> new RevenuePaymentRowDto
                {
                    PaymentId = p.Id,
                    PaidAt = p.CreatedAt,
                    InvoiceId = p.InvoiceId,
                    PatientFullName = p.Invoice.Patient.FullName,
                    Method = p.Method.ToString(),
                    Amount = p.Amount
                })
                .ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Reports;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;

namespace ClinicManagementSystem.Infrastructure.Documents
{
    public class RevenueReportService : IRevenueReportService
    {
        private const string XlsxContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly IReportRepository _reportRepository;
        private readonly IRevenueReportGenerator _generator;

        public RevenueReportService(
            IReportRepository reportRepository,
            IRevenueReportGenerator generator)
        {
            _reportRepository = reportRepository;
            _generator = generator;
        }
        public async Task<RevenueReportModel> BuildModelAsync(DateTime periodStart, DateTime periodEnd)
        {
            var startDate = periodStart.Date;
            var endDate = periodEnd.Date;

            var from = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var to = DateTime.SpecifyKind(endDate, DateTimeKind.Utc).AddDays(1);

            var payments = await _reportRepository.GetPaymentsInRangeAsync(from, to);
            var paymentsByDate = payments.ToLookup(p => p.PaidAt.Date);

            var daysCount = (endDate - startDate).Days + 1;

            return new RevenueReportModel
            {
                PeriodStart = startDate,
                PeriodEnd = endDate,
                TotalRevenue = payments.Sum(p => p.Amount),
                PaymentsCount = payments.Count,
                Payments = payments,
                ByMethod = payments
                    .GroupBy(p => p.Method)
                    .Select(g => new RevenueByMethodItem
                    {
                        Method = g.Key,
                        PaymentsCount = g.Count(),
                        Amount = g.Sum(p => p.Amount)
                    })
                    .OrderByDescending(x => x.Amount)
                    .ToList(),
                ByDay = Enumerable.Range(0, daysCount)
                    .Select(offset =>
                    {
                        var date = startDate.AddDays(offset);
                        var dayPayments = paymentsByDate[date].ToList();
                        return new RevenueByDayItem
                        {
                            Date = date,
                            PaymentsCount = dayPayments.Count,
                            Amount = dayPayments.Sum(p => p.Amount)
                        };
                    })
                    .ToList()
            };
        }

        public ReportFileResult ToExcelFile(RevenueReportModel model, string fileName)
        {
            return new ReportFileResult
            {
                Content = _generator.Generate(model),
                ContentType = XlsxContentType,
                FileName = fileName
            };
   }  }
}

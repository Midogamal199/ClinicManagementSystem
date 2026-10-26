using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Reports;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicManagementSystem.Application.Features.Reports.Queries.GetMonthlyRevenueReport
{
    public class GetMonthlyRevenueReportQueryHandler : IRequestHandler<GetMonthlyRevenueReportQuery, ReportFileResult>
    {
        private const string XlsxContentType =
           "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IReportRepository _reportRepository;
        private readonly IMonthlyRevenueReportGenerator _generator;

        public GetMonthlyRevenueReportQueryHandler(
            IReportRepository reportRepository,
            IMonthlyRevenueReportGenerator generator)
        {
            _reportRepository = reportRepository;
            _generator = generator;
        }
        public async Task<ReportFileResult> Handle(GetMonthlyRevenueReportQuery request, CancellationToken cancellationToken)
        {
            var from = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = from.AddMonths(1);
            var payments = await _reportRepository.GetPaymentsInRangeAsync(from, to);
            var paymentByDate= payments.ToLookup(p=> p.PaidAt.Date);
            var model = new MonthlyRevenueReportModel
            {
                Year = request.Year,
                Month = request.Month,
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
                ByDay=Enumerable.Range(1, DateTime.DaysInMonth(request.Year, request.Month))
                .Select(day =>
                {
                    var date = new DateTime(request.Year, request.Month, day);
                    var dayPayments = paymentByDate[date].ToList();
                    return new RevenueByDayItem
                    {
                        Date = date,
                        PaymentsCount = dayPayments.Count,
                        Amount = dayPayments.Sum(p => p.Amount)
                    };

                }).ToList()

            };
            return new ReportFileResult
            {
                Content = _generator.Generate(model),
                ContentType = XlsxContentType,
                FileName = $"revenue-{request.Year}-{request.Month:00}.xlsx"
            };
        }
    }
}

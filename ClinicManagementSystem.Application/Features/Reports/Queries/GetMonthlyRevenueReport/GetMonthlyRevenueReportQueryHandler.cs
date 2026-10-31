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
        private readonly IRevenueReportService _reportService;


        public GetMonthlyRevenueReportQueryHandler(IRevenueReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<ReportFileResult> Handle(GetMonthlyRevenueReportQuery request, CancellationToken cancellationToken)
        {
            var periodStart = new DateTime(request.Year, request.Month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            var model = await _reportService.BuildModelAsync(periodStart, periodEnd);

            return _reportService.ToExcelFile(model, $"revenue-{request.Year}-{request.Month:00}.xlsx");
        }
    }
}

using ClinicManagementSystem.Application.DTOs.Reports;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Reports.Queries.GetMonthlyRevenueReport
{
    public class GetMonthlyRevenueReportQuery : IRequest<ReportFileResult>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
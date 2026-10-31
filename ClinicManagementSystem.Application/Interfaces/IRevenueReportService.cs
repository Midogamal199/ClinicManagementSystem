using System;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Reports;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IRevenueReportService
    {
        Task<RevenueReportModel> BuildModelAsync(DateTime periodStart, DateTime periodEnd);
        ReportFileResult ToExcelFile(RevenueReportModel model, string fileName);
    }
}
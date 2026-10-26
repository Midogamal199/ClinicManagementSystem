using ClinicManagementSystem.Application.DTOs.Reports;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IMonthlyRevenueReportGenerator
    {
        byte[] Generate(MonthlyRevenueReportModel model);
    }
}
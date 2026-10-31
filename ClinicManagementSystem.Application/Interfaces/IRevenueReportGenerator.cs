using ClinicManagementSystem.Application.DTOs.Reports;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IRevenueReportGenerator
    {
        byte[] Generate(RevenueReportModel model);
    }
}
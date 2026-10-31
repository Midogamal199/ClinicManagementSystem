using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IScheduledReportsJob
    {
        Task SendWeeklyRevenueReportAsync();
        Task SendMonthlyRevenueReportAsync();
    }
}
namespace ClinicManagementSystem.Domain.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TodayVisits { get; set; }
        public int TodayAppointments { get; set; }
        public decimal TodayRevenue { get; set; }
    }
}
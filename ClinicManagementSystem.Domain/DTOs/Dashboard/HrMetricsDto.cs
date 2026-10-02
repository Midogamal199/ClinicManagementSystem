namespace ClinicManagementSystem.Domain.DTOs.Dashboard
{
    public class HrMetricsDto
    {
        public int CurrentlyPresentCount { get; set; }
        public int PendingLeaveRequestsCount { get; set; }
        public List<DepartmentDistributionDto> DepartmentDistribution { get; set; } = new();
    }

    public class DepartmentDistributionDto
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
    }
}
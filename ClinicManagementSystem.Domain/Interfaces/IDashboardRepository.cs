using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;



namespace ClinicManagementSystem.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetTodayVisitsCountAsync();
        Task<int> GetTodayAppointmentsCountAsync();
        Task<decimal> GetTodayRevenueAsync();
        Task<List<DiagnosisTrendItemDto>> GetDiagnosisTrendsAsync(DateTime fromDate, int top = 10);
        Task<List<RevenueTrendItemDto>> GetRevenueTrendAsync(DateTime fromDate);
        Task<int> GetCurrentlyPresentCountAsync();
        Task<int> GetPendingLeaveRequestsCountAsync();
        Task<List<DepartmentDistributionDto>> GetDepartmentDistributionAsync();
        Task<List<TopVisitedDoctorDto>> GetTopVisitedDoctorsAsync(int top = 5);
        Task<List<GenderDistributionItemDto>> GetGenderDistributionAsync();
        Task<List<SpecialtyDistributionItemDto>> GetSpecialtyDistributionAsync();
        Task<List<FrequentVisitorDto>> GetFrequentVisitorsAsync(int top = 10);
    }
}
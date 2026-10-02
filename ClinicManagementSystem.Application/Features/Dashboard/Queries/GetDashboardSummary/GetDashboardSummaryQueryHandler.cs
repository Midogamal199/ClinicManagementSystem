using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDashboardSummary
{

    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetDashboardSummaryQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"dashboard:summary:{DateTime.UtcNow:yyyy-MM-dd}";
            if (_cache.TryGetValue(cacheKey, out DashboardSummaryDto? cached) && cached is not null)
            {
                return cached;
            }
            var result = new DashboardSummaryDto
            {
                TodayVisits = await _dashboardRepository.GetTodayVisitsCountAsync(),
                TodayAppointments = await _dashboardRepository.GetTodayAppointmentsCountAsync(),
                TodayRevenue = await _dashboardRepository.GetTodayRevenueAsync()
            };
            _cache.Set(cacheKey, result, CacheDuration);
            return result;
        }
    }
}

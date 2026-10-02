using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetHrMetrics
{
    public class GetHrMetricsQueryHandler : IRequestHandler<GetHrMetricsQuery, HrMetricsDto>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:hr-metrics";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetHrMetricsQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<HrMetricsDto> Handle(GetHrMetricsQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out HrMetricsDto? cached) && cached is not null)
            {
                return cached;
            }
            var result = new HrMetricsDto
            {
                CurrentlyPresentCount = await _dashboardRepository.GetCurrentlyPresentCountAsync(),
                PendingLeaveRequestsCount = await _dashboardRepository.GetPendingLeaveRequestsCountAsync(),
                DepartmentDistribution = await _dashboardRepository.GetDepartmentDistributionAsync()
            };
            _cache.Set(CacheKey, result, CacheDuration);
            return result;

        }
    }
}

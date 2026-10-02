using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetRevenueTrend
{
    public class GetRevenueTrendQueryHandler : IRequestHandler<GetRevenueTrendQuery, List<RevenueTrendItemDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:revenue-trend";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetRevenueTrendQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<List<RevenueTrendItemDto>> Handle(GetRevenueTrendQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<RevenueTrendItemDto>? cached) && cached is not null)
            {
                return cached;
            }
            var fromDate = DateTime.UtcNow.AddDays(-30);
            var result = await _dashboardRepository.GetRevenueTrendAsync(fromDate);
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

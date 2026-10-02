using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetDiagnosisTrends
{
    public class GetDiagnosisTrendsQueryHandler : IRequestHandler<GetDiagnosisTrendsQuery, List<DiagnosisTrendItemDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:diagnosis-trends";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        public GetDiagnosisTrendsQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<List<DiagnosisTrendItemDto>> Handle(GetDiagnosisTrendsQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<DiagnosisTrendItemDto>? cached) && cached is not null)
            {
                return cached;
            }
            var fromDate = DateTime.UtcNow.Date.AddDays(-30);
            var result = await _dashboardRepository.GetDiagnosisTrendsAsync(fromDate);
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

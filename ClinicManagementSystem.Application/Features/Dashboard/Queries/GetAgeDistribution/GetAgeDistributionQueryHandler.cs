using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetAgeDistribution
{
    public class GetAgeDistributionQueryHandler : IRequestHandler<GetAgeDistributionQuery, List<AgeDistributionItemDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:age-distribution";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetAgeDistributionQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }

        public async Task<List<AgeDistributionItemDto>> Handle(GetAgeDistributionQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<AgeDistributionItemDto>? cached) && cached is not null)
            {
                return cached;
            }
            var result = await _dashboardRepository.GetAgeDistributionAsync();
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

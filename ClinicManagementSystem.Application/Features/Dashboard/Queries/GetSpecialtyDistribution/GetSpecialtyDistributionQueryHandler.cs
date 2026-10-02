using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetSpecialtyDistribution
{
    public class GetSpecialtyDistributionQueryHandler : IRequestHandler<GetSpecialtyDistributionQuery, List<SpecialtyDistributionItemDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:specialty-distribution";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetSpecialtyDistributionQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<List<SpecialtyDistributionItemDto>> Handle(GetSpecialtyDistributionQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<SpecialtyDistributionItemDto>? cached) && cached is not null)
            {
                return cached;
            }
            var result = await _dashboardRepository.GetSpecialtyDistributionAsync();
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetGenderDistribution
{
    public class GetGenderDistributionQueryHandler : IRequestHandler<GetGenderDistributionQuery, List<GenderDistributionItemDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:gender-distribution";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetGenderDistributionQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<List<GenderDistributionItemDto>> Handle(GetGenderDistributionQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<GenderDistributionItemDto>? cached) && cached is not null)
            {
                return cached;
            }
            var result= await _dashboardRepository.GetGenderDistributionAsync();
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

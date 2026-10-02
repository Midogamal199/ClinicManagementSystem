using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetFrequentVisitors
{
    public class GetFrequentVisitorsQueryHandler : IRequestHandler<GetFrequentVisitorsQuery, List<FrequentVisitorDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:frequent-visitors";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetFrequentVisitorsQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }

        public async Task<List<FrequentVisitorDto>> Handle(GetFrequentVisitorsQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<FrequentVisitorDto>? cached) && cached is not null)
            {
                return cached;
            }
            var result = await _dashboardRepository.GetFrequentVisitorsAsync();
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

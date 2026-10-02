using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.DTOs.Dashboard;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicManagementSystem.Application.Features.Dashboard.Queries.GetTopVisitedDoctors
{
    public class GetTopVisitedDoctorsQueryHandler : IRequestHandler<GetTopVisitedDoctorsQuery, List<TopVisitedDoctorDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "dashboard:top-doctors";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        public GetTopVisitedDoctorsQueryHandler(IDashboardRepository dashboardRepository, IMemoryCache cache)
        {
            _dashboardRepository = dashboardRepository;
            _cache = cache;
        }
        public async Task<List<TopVisitedDoctorDto>> Handle(GetTopVisitedDoctorsQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<TopVisitedDoctorDto>? cached) && cached is not null)
            {
                return cached;
            }
            var result = await _dashboardRepository.GetTopVisitedDoctorsAsync();
            _cache.Set(CacheKey, result, CacheDuration);
            return result;
        }
    }
}

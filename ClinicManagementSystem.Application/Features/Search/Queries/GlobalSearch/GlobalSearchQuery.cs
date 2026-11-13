using ClinicManagementSystem.Application.DTOs.Search;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Search.Queries.GlobalSearch
{
    public class GlobalSearchQuery : IRequest<GlobalSearchResultDto>
    {
        public string Term { get; set; } = string.Empty;
        public int Limit { get; set; } = 5;
    }
}
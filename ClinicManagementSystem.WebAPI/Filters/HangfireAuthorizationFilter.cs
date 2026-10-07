using ClinicManagementSystem.Infrastructure.Identity;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;

namespace ClinicManagementSystem.WebAPI.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var result = httpContext.AuthenticateAsync("HangfireAuth").GetAwaiter().GetResult();
            return result.Succeeded
                && result.Principal is not null
                && result.Principal.IsInRole(Roles.Admin);
        }
    }
}

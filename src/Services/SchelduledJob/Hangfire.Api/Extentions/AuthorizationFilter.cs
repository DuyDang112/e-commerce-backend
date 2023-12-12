using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Hangfire.Api.Extentions
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context) => true;
    }
}

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.Configurations;
using Infrastructures.Extentions;

namespace Customer.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            services.AddHealthChecks()
                .AddNpgSql(databaseSettings.ConnectionString,
                name: "Posgres Health",
                failureStatus: HealthStatus.Degraded);
        }
    }
}

using Shared.Configurations;

namespace Hangfire.Api.Extentions
{
    public static class HostExtention
    {
        public static void AddAppCongigurations(this ConfigureHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            }
            );
        }

        // override UseHangfireDashboard of Hangfire
        internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app,
            IConfiguration configuration)
        {
            var configDashboard = configuration.GetSection("HangFireSettings:Dashboard").Get<DashboardOptions>();
            var hangfireSettings = configuration.GetSection("HangFireSettings").Get<HangFireSettings>();
            var hangfireRoute = hangfireSettings.Route;

            app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
            {
                Authorization = new[] { new AuthorizationFilter() },
                DashboardTitle = configDashboard.DashboardTitle,
                StatsPollingInterval = configDashboard.StatsPollingInterval,
                AppPath = configDashboard.AppPath,
                IgnoreAntiforgeryToken = true
            });

            return app;
        }
    }
}

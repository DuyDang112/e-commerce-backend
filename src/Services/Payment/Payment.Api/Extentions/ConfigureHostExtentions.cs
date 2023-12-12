namespace Payment.Api.Extentions
{
    public static class ConfigureHostExtentions
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
    }
}

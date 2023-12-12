using Contract.Common.Interfaces;
using Infrastructures.Common;
using Infrastructures.Extentions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Shared.Configurations;

namespace OcelotApiGw.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration)
                .AddPolly()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration["AllowOrigins"];
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", buider =>
                {
                    buider.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }

    
}

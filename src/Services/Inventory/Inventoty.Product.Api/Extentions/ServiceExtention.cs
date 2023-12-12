using Shared.Configurations;
using Infrastructures.Extentions;
using MongoDB.Driver;
using Inventoty.Product.Api.Services.Interfaces;
using Inventoty.Product.Api.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using RabbitMQ.Client;
using System.Reflection;
using Inventoty.Product.Api.CourierActivities;

namespace Inventoty.Product.Api.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(MongoDbSettings))
                .Get<MongoDbSettings>();

            services.AddSingleton(databaseSettings);

            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            return services;
        }

        private static string getMongoDbConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
            if (string.IsNullOrEmpty(settings.ConnectionString) || settings == null) 
                throw new ArgumentNullException("MongoDbSetting is not configured");
            var databaseName = settings.DatabaseName;
            var mongoDbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";
            return mongoDbConnectionString;
        }
        public static void ConfigureMongoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(
                new MongoClient(getMongoDbConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
        }
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
           services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
           services.AddScoped<IInventoryService,InventoryService>();
        }

        public static void ConfigureNasstransit(this IServiceCollection services)
        {
            //using var serviceProvider = services.BuildServiceProvider(); ==> serviceProvider.GetRequireService<>();
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSetting is not configured");

            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddActivitiesFromNamespaceContaining<AllocateInventoryActivity>();
                config.AddConsumers(Assembly.GetExecutingAssembly());
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);

                    cfg.ConfigureEndpoints(ctx); // auto create queue, exchange, endpoint so that  binded 
                });
            });
        }
    }
}

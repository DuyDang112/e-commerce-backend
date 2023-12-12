using Shared.Configurations;
using Infrastructures.Extentions;
using MongoDB.Driver;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using RabbitMQ.Client;
using System.Reflection;
using Shipment.Api.CourierActivities;

namespace Shipment.Api.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            return services;
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
        }

        public static void ConfigureNasstransit(this IServiceCollection services)
        {
            //using var serviceProvider = services.BuildServiceProvider(); ==> serviceProvider.GetRequireService<>();
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSetting is not configured");

            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddActivitiesFromNamespaceContaining<ShipmentActivity>();
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

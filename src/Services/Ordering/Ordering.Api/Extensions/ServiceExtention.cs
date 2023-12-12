using Infrastructures.Configurations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;
using Infrastructures.Extentions;
using MassTransit;
using System.Reflection;
using Ordering.Api.Aplication.IntergrationEvents.CommandsHandler;
using RabbitMQ.Client;
using MassTransit.RabbitMqTransport;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;

namespace Ordering.Api.Extensions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting))
                .Get<SMTPEmailSetting>();

            services.AddSingleton(emailSettings);

            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            return services;
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
                config.AddConsumers(Assembly.GetExecutingAssembly());
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);

                    cfg.ReceiveEndpoint("submit-order", e =>
                    {
                        e.ConfigureConsumer<SubmitOrderConsumer>(ctx);
                    });

                    //cfg.ConfigureEndpoints(ctx); // auto receive all endPoints when have handler consume an event same same 
                });

                config.AddRequestClient<CheckOrder>();
            });
        }
    }
}

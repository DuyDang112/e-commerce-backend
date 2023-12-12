using Contract.Common.Interfaces;
using Infrastructures.Common;
using Infrastructures.Extentions;
using Infrastructures.Sagas;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Saga.Orchestrator.Workflow.Consumer;
using Saga.Orchestrator.Workflow.OrderSaga.Masstransit;
using Saga.Orchestrator.Workflow.OrderSaga.Masstransit.OrderStateMachineActivities;
using Serilog;
using Shared.Configurations;

namespace Saga.Orchestrator.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
            services.AddScoped<AcceptOrderActivity>();


        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            return services;
        }

        public static void ConfigureMassTranSit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (string.IsNullOrEmpty(settings.HostAddress)) 
                throw new ArgumentNullException("EventBusSetting is not configured");
            
            var mqConnection = new  Uri(settings.HostAddress);
            //var sagaStateMachine = new OrderStateMachine(Log.Logger);

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<FulfillOrderConsumer>();
                config.AddSagaStateMachine<OrderStateMachine,OrderSagaState>()
                    .MongoDbRepository(r =>
                    {
                        r.Connection = "mongodb://127.0.0.1";
                        r.DatabaseName = "SagaDb";
                        r.CollectionName = "OrderSagaDb";
                    });
                config.UsingRabbitMq((ctx,cfg) =>
                {
                    cfg.Host(mqConnection);



                    cfg.ConfigureEndpoints(ctx);
                });
            });
            

        }
    }

    
}

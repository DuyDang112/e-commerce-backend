using Basket.Api.GrpcServices;
using Basket.Api.Repositoties;
using Basket.Api.Repositoties.Interface;
using Basket.Api.Services;
using Basket.Api.Services.Interfaces;
using Contract.Common.Interfaces;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using Infrastructures.Common;
using Infrastructures.Extentions;
using Inventory.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Shared.Configurations;

namespace Basket.Api.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services) => 
            services.AddScoped<IBasketRepository,BasketRepository>()
            .AddTransient<ISerializeService,SerializeService>()
            .AddTransient<IEmailTemplateService,BasketEmaiTemplateService>();


        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            var cacheSettings = configuration.GetSection(nameof(CacheSettings))
                .Get<CacheSettings>();

            services.AddSingleton(cacheSettings);

            var grpcSettings = configuration.GetSection(nameof(GrpcSettings))
                .Get<GrpcSettings>();

            services.AddSingleton(grpcSettings);

            var backgroundJobsettings = configuration.GetSection(nameof(BackgroundJobSettings))
            .Get<BackgroundJobSettings>();

            services.AddSingleton(backgroundJobsettings);

            return services;
        }

        public static void ConfigureRedis(this IServiceCollection services)
        {
            var settings = services.GetOptions<CacheSettings>(nameof(CacheSettings));
            if (string.IsNullOrEmpty(settings.ConnectionString)) 
                throw new ArgumentNullException("Redis connection string is not configured");

            services.AddStackExchangeRedisCache(otps =>
            {
                otps.Configuration = settings.ConnectionString;
            });
        }

        public static IServiceCollection ConfigureGrpc(this IServiceCollection services)
        {
            var settings = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));
            services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(x => x.Address = new Uri(settings.StockUrl));

            services.AddScoped<StockItemGrpcService>();

            return services;
        }

        public static IServiceCollection ConfigureHttpClientService(this IServiceCollection services)
        {
            services.AddHttpClient<BackgroundJobHttpService>();
            return services;
        }

        public static void ConfigureMassTranSit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (string.IsNullOrEmpty(settings.HostAddress)) 
                throw new ArgumentNullException("EventBusSetting is not configured");
            
            var mqConnection = new  Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx,cfg) =>
                {
                    cfg.Host(mqConnection);

                });

                var BasketCheckoutConsumer = "submit-order";

                config.AddRequestClient<SubmitOrder>(new 
                    Uri($"queue:{BasketCheckoutConsumer}"));
            });
            

        }
    }

    
}

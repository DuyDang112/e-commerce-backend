using Shared.Configurations;
using MongoDB.Driver;
using Infrastructures.Extentions;
using Inventory.Grpc.Repository.Interfaces;
using Inventory.Grpc.Repository;

namespace Inventory.Grpc.Extentions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(MongoDbSettings))
                .Get<MongoDbSettings>();

            services.AddSingleton(databaseSettings);

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
           services.AddScoped<IInventoryRepository,InventoryRepository>();
        }
    }
}

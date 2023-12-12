using Inventoty.Product.Api.Persistene;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventoty.Product.Api.Extentions
{
    // Ihost: quản lý các dịch vụ, cấu hình ứng dụng và tạo ra một môi trường thực thi cho ứng dụng chạy.
    public static class HostExtention
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var settings = scope.ServiceProvider.GetService<MongoDbSettings>();
            if (string.IsNullOrEmpty(settings?.ConnectionString) || settings == null)
                throw new ArgumentNullException("DatabaseSetting MongoDb is not configured");
            var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();

            new InventoryDbSeed()
                .SeedDataAsync(mongoClient, settings).Wait();
            return host;
        }
    }
}

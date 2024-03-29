﻿using Microsoft.EntityFrameworkCore;
using Polly;

namespace Product.Api.Extentions
{
    // Ihost: quản lý các dịch vụ, cấu hình ứng dụng và tạo ra một môi trường thực thi cho ứng dụng chạy.
    public static class HostExtention
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext,IServiceProvider> seeder )
        where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating sql database");
                    ExecuteMigrations(context);
                    logger.LogInformation("Migrated sql database");
                    InvokeSeeder(seeder, context, services);
                }
                catch
                (Exception ex)
                {
                    logger.LogInformation($"{ex.Message}, An error occured while migrating the mysql database");
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            seeder(context, services);
        }

        private static void ExecuteMigrations<TContext>(TContext context) where TContext : DbContext
        {
            //context.Database.Migrate(); mySql
            if (context.Database.IsSqlServer())
            {
                context.Database.MigrateAsync();
            }
        }
    }
}

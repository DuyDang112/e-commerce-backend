using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.PostgreSql;
using Infrastructures.Extentions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.ScheduleJobs
{
    public static class HangfireExtentions
    {
        public static IServiceCollection AddHangfireService(this  IServiceCollection services)
        {
            var settings = services.GetOptions<HangFireSettings>(nameof(HangFireSettings));
            if (settings == null ||settings.Storage == null ||
                string.IsNullOrEmpty(settings.Storage.ConnectionString))
                throw new ArgumentNullException("HangFireSettings  is not configured");

            services.ConfigureHangfireServices(settings);
            services.AddHangfireServer(serverOption =>
            {
                serverOption.ServerName = settings.ServerName;
            });

            return services;
        }

        private static IServiceCollection ConfigureHangfireServices(this IServiceCollection services,
            HangFireSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Storage.DbProvider))
                throw new ArgumentNullException("Hangfire DbProvider is not configured");

            switch (settings.Storage.DbProvider.ToLower())
            {
                case "mongodb":
                    var mongoUrlBuilder = new MongoUrlBuilder(settings.Storage.ConnectionString);

                    var mongoClientSettings = MongoClientSettings.FromUrl(
                        new MongoUrl(settings.Storage.ConnectionString));
                    mongoClientSettings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                    };
                    var mongoClient = new MongoClient(mongoClientSettings);

                    var mongoStorageOptions = new MongoStorageOptions
                    {
                        MigrationOptions = new MongoMigrationOptions
                        {
                            MigrationStrategy = new MigrateMongoMigrationStrategy(),
                            BackupStrategy = new CollectionMongoBackupStrategy(),
                        },
                        CheckConnection = true,
                        Prefix = "SchedulerQueue",
                        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                    };

                    services.AddHangfire((provider, config) =>
                    {
                        config.UseSimpleAssemblyNameTypeSerializer()
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseRecommendedSerializerSettings()
                        .UseConsole()
                        .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

                        var jsonSettings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        };
                        config.UseSerializerSettings(jsonSettings);
                    });

                    services.AddHangfireConsoleExtensions();

                    break;
                case "postgresql":
                    services.AddHangfire(x =>
                    {
                        x.UsePostgreSqlStorage(settings.Storage.ConnectionString);
                    });
                    break;
                case "mssql":
                    break;

                default:
                    throw new ArgumentException($"Hangfire DbProvider {settings.Storage.DbProvider} is not supported");
            }
            return services;
        }
    }
}

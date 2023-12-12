using Contract.Common.Interfaces;
using Contract.ScheduledJobs;
using Contract.Services;
using Hangfire.Api.Services;
using Hangfire.Api.Services.Interfaces;
using Infrastructures.Common;
using Infrastructures.Configurations;
using Infrastructures.ScheduleJobs;
using Infrastructures.Services;
using Shared.Configurations;
using System;

namespace Hangfire.Api.Extentions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var hangFireSettings = configuration.GetSection(nameof(HangFireSettings))
                .Get<HangFireSettings>();

            services.AddSingleton(hangFireSettings);

            var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting))
                .Get<SMTPEmailSetting>();

            services.AddSingleton(emailSettings);

            return services;
        }

        public static IServiceCollection ConfigureServices(this  IServiceCollection services)
        {
            services.AddTransient<IScheduledJobService, HangfireService>()
                .AddTransient<IBackGroundJobService, BackGroundJobService>()
                .AddScoped<ISmtpEmailService, SmtpEmailService>();
            return services;
        }
    }
}

using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Behaviours;
using Ordering.Application.Features.V1.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services.AddAutoMapper(Assembly.GetExecutingAssembly()) //auto mapping , No config profile
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandleExceptionBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
            ;
    }
}

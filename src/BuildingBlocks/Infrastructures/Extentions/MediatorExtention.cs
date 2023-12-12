using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Contract.Common.Interfaces;
using Infrastructures.Common;
using Contract.Common.Events;

namespace Infrastructures.Extentions
{
    public static class MediatorExtention
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator, 
            List<BaseEvent> domainEvents, 
            ILogger logger)
        {
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
                var data = new SerializeService().Serialize(domainEvent);
                logger.Information($"\n----\nA domain event has been published!\n" +
                    $"Event: {domainEvent.GetType().Name}\n" +
                    $"Data: {data}\n----\n");
            }
        }
    }
}

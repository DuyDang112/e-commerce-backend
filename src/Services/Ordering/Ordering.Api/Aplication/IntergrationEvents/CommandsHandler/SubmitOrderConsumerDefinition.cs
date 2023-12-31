﻿using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using MassTransit;

namespace Ordering.Api.Aplication.IntergrationEvents.CommandsHandler
{
    public class SubmitOrderConsumerDefinition : ConsumerDefinition<SubmitOrderConsumer>
    {
        readonly IServiceProvider _serviceProvider;
        public SubmitOrderConsumerDefinition(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            ConcurrentMessageLimit = 10;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
          IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator,
          IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
            endpointConfigurator.UseServiceScope(_serviceProvider);

            endpointConfigurator.UseInMemoryOutbox(context);
            //consumerConfigurator.Message<SubmitOrder>(m => m.UseFilter(new ContainerScopedFilter()));
        }
    }
}

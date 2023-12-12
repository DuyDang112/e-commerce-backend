using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using MassTransit;

namespace Saga.Orchestrator.Workflow.Consumer
{
    public class FaultFulfillOrderConsumer : IConsumer<Fault<FulfillOrder>>
    {
        public Task Consume(ConsumeContext<Fault<FulfillOrder>> context)
        {
            return Task.CompletedTask;
        }
    }
}

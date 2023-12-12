using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Infrastructures.Sagas;
using MassTransit;

namespace Saga.Orchestrator.Workflow.OrderSaga.Masstransit
{
    public class OrderStateMachineDefinition : SagaDefinition<OrderSagaState>
    {
        public OrderStateMachineDefinition()
        {
            ConcurrentMessageLimit = 12;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderSagaState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}

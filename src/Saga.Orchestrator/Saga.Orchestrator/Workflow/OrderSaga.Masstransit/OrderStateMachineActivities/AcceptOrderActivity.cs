using Automatonymous;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Infrastructures.Sagas;
using MassTransit;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Workflow.OrderSaga.Masstransit.OrderStateMachineActivities
{
    public class AcceptOrderActivity :
        IStateMachineActivity<OrderSagaState, OrderAccepted>
    {
        private readonly ILogger _logger;

        public AcceptOrderActivity(ILogger logger)
        {
            _logger = logger;
        }
        public void Probe(ProbeContext context)
        {
            context.CreateScope("accept-order");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderSagaState, OrderAccepted> context, IBehavior<OrderSagaState, OrderAccepted> next)
        {
            _logger.Information($"Execute Order: {context.Data.OrderId}");

            var consumeContext = context.GetPayload<ConsumeContext>();

            var sendEndpoint = await consumeContext.GetSendEndpoint(new Uri("queue:fulfill-order"));

            await sendEndpoint.Send<FulfillOrder>(new
            {
                context.Data.OrderId,
                context.Instance.UserName,
                context.Instance.PaymentCardNumber,
                context.Instance.TotalAmount,
                context.Instance.ShipmentAddress,
                context.Instance.PhoneNumber,
                context.Instance.Items
            });

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderSagaState, OrderAccepted, TException> context, IBehavior<OrderSagaState, OrderAccepted> next) where TException : Exception
        {
            return next.Faulted(context);
        }

    }
}

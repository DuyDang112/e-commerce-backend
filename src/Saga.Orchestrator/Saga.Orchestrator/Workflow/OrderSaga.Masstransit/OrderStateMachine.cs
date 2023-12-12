using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Infrastructures.Sagas;
using MassTransit;
using Saga.Orchestrator.Workflow.OrderSaga.Masstransit.OrderStateMachineActivities;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Workflow.OrderSaga.Masstransit
{
    public class OrderStateMachine : MassTransitStateMachine<OrderSagaState>
    {

        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        public State Allocated { get; private set; }
        public State Transported { get; private set; }
        public State Canceled { get; private set; }
        public State FullfillFaulted { get; private set; }
        public State FullFillCompleted { get; private set; }

        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<OrderAccepted> OrderAccepted { get; private set; }
        public Event<CustomerAccountClosed> AccountClosed { get; private set; }
        public Event<InventoryAllocated> InventoryAllocated { get; private set; }
        public Event<OrderFulfillmentCompleted> FulfillmentCompleted { get; private set; }
        public Event<OrderFulfillmentFaulted> FulfillmentFaulted { get; private set; }
        public Event<Fault<FulfillOrder>> FulfillOrderFaulted { get; private set; }
        public Event<CheckOrder> OrderStatusRequested { get; private set; }

        public OrderStateMachine()
        {

            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderAccepted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => InventoryAllocated, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => FulfillmentCompleted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => FulfillmentFaulted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => FulfillOrderFaulted, x => x.CorrelateById(m => m.Message.Message.OrderId));
            Event(() => AccountClosed, x => x.CorrelateBy((saga, context) => saga.UserName == context.Message.UserName));
            Event(() => OrderStatusRequested, x =>
            {
                x.CorrelateById(m => m.Message.OrderId);
                x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    if (context.RequestId.HasValue) // if has message published
                        await context.RespondAsync<OrderNotFound>(new { context.Message.OrderId });
                }));
            });

            InstanceState(x => x.CurrentState);
            // When basket checkout
            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate = context.Data.Timestamp;
                        context.Instance.UserName = context.Data.UserName;
                        context.Instance.PaymentCardNumber = context.Data.PaymentCardNumber;
                        context.Instance.TotalAmount = (double)context.Data.TotalAmount;
                        context.Instance.ShipmentAddress = context.Data.ShipmentAddress;
                        context.Instance.PhoneNumber = context.Data.PhoneNumber;
                        context.Instance.Items = context.Data.OrdersItem.ToList().Select(item => new OrderSagaState.OrdersItem
                        {
                            ProductNo = item.ProductNo,
                            Quantity = item.Quantity
                        }).ToList();
                        context.Instance.Updated = DateTime.UtcNow;
                    })
                    .TransitionTo(Submitted));

            During(Submitted,
                Ignore(OrderSubmitted),
                When(AccountClosed)
                    .TransitionTo(Canceled),
                When(OrderAccepted)
                    .Activity(x => x.OfType<AcceptOrderActivity>())
                    .TransitionTo(Accepted));

            During(Accepted,
                When(FulfillOrderFaulted)
                    //.Then(context => _logger.Information("Fulfill Order Faulted: {0}", context.Data.Exceptions.FirstOrDefault()?.Message))
                    .TransitionTo(FullfillFaulted),
                When(FulfillmentFaulted)
                    .TransitionTo(FullfillFaulted),
                When(FulfillmentCompleted)
                    .TransitionTo(FullFillCompleted));
                

            During(FullFillCompleted,
                Ignore(OrderAccepted),
                When(InventoryAllocated)
                    .TransitionTo(Allocated)
                );


            DuringAny(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate ??= context.Data.Timestamp;
                        context.Instance.UserName ??= context.Data.UserName;
                    }),
                When(OrderStatusRequested)
                    .RespondAsync(x => x.Init<OrderStatus>(new
                    {
                        OrderId = x.Instance.CorrelationId,
                        State = x.Instance.CurrentState
                    }))
            );
        }

    }
}

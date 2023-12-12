using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace Saga.Orchestrator.Workflow.Consumer
{
    public class FulfillOrderConsumer : IConsumer<FulfillOrder>
    {
        public async Task Consume(ConsumeContext<FulfillOrder> context)
        {
            if (context.Message.UserName.StartsWith("INVALID"))
            {
                throw new InvalidOperationException("We tried, but the customer is invalid");
            }

            if (context.Message.UserName.StartsWith("MAYBE"))
                if (new Random().Next(100) > 90)
                    throw new ApplicationException("we randomly explored");

            ////// Implement Idempotence pattern to handle 2 same message when Outbox pattern has problem
            //var MessageId = context.MessageId;

            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity("AllocateInventory", new Uri("queue:allocate-inventory_execute"), new
            {
                context.Message.OrderId,
                context.Message.Items
            });

            builder.AddActivity("PaymentActivity", new Uri("queue:payment_execute"),
                new
                {
                    OrderId = context.Message.OrderId,
                    CardNumber = context.Message.PaymentCardNumber ?? "5999-1234-5678-9012",
                    TotalAmount = context.Message.TotalAmount
                });

            builder.AddActivity("ShipmentActivity", new Uri("queue:shipment_execute"),
               new
               {
                   OrderId = context.Message.OrderId,
                   ShipmmentAddress = context.Message.ShipmentAddress,
                   PhoneNumber = context.Message.PhoneNumber
               });

            builder.AddVariable("OrderId", context.Message.OrderId);

            await builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.None, x => x.Send<OrderFulfillmentFaulted>(new { context.Message.OrderId }));

            await builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.None, x => x.Send<OrderFulfillmentCompleted>(new { context.Message.OrderId }));

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }
    }
}

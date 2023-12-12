using MassTransit;

namespace Saga.Orchestrator.Workflow.Consumer
{
    public class FulfillOrderConsumerDefinition :ConsumerDefinition<FulfillOrderConsumer>
    {
        public FulfillOrderConsumerDefinition()
        {
            ConcurrentMessageLimit = 20;
        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<FulfillOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r =>
            {
                //bỏ qua các exception invalid để retry lại xử lí những việc khác 
                r.Ignore<InvalidOperationException>();

                r.Interval(3, 1000);
            });

            // đã theo dõi lỗi trong saga và xóa các message lỗi trong hàng đợi error
            endpointConfigurator.DiscardFaultedMessages();
        }
    }
}

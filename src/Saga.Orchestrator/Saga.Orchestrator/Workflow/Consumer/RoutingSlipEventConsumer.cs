using MassTransit;
using MassTransit.Courier.Contracts;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Workflow.Consumer
{
    public class RoutingSlipEventConsumer : IConsumer<RoutingSlipFaulted>, IConsumer<RoutingSlipActivityCompleted>
    {
        private readonly ILogger _logger;

        public RoutingSlipEventConsumer(ILogger logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompleted> context)
        {
                _logger.Information("Routing Slip Activity Completed: {TrackingNumber} {ActivityName}", context.Message.TrackingNumber,
                    context.Message.ActivityName);

            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
                _logger.Information("Routing Slip Faulted: {TrackingNumber} {ExceptionInfo}", context.Message.TrackingNumber,
                    context.Message.ActivityExceptions.FirstOrDefault());

            return Task.CompletedTask;
        }
    }
}

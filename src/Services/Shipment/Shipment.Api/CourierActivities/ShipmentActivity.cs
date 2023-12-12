using MassTransit;
using ILogger = Serilog.ILogger;

namespace Shipment.Api.CourierActivities
{
    public class ShipmentActivity : IActivity<ShipmentArguments,ShipmentLog>
    {
        private readonly ILogger _logger;

        public ShipmentActivity(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ShipmentArguments> context)
        {
            var shippingId = Guid.NewGuid();
            _logger.Information($"------- Executed Shipping with OrderId: {context.Arguments.OrderId}-----------------");
            return context.Completed(new { ShippingId = shippingId });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ShipmentLog> context)
        {
            _logger.Information($"--------Compensated ShippingID: {context.Log.ShippingId}------------");

            return context.Compensated();
        }
    }
}

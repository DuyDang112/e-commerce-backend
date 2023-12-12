using MassTransit;

namespace Shipment.Api.CourierActivities
{
    public class ShipmentActivityDefinition : 
        ActivityDefinition<ShipmentActivity, ShipmentArguments, ShipmentLog>
    {
        public ShipmentActivityDefinition()
        {
            ConcurrentMessageLimit = 20;
        }
    }
}

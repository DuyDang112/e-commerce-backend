using MassTransit;

namespace Inventoty.Product.Api.CourierActivities
{
    public class AllocateInventoryActivityDefinition : 
        ActivityDefinition<AllocateInventoryActivity, AllocateInventoryArguments, AllocateInventoryLog>
    {
        public AllocateInventoryActivityDefinition()
        {
            ConcurrentMessageLimit = 10;
        }
    }
}

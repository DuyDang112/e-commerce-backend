namespace Shipment.Api.CourierActivities
{
    public interface ShipmentArguments
    {
        string OrderId { get;}
        string ShipmentAddress { get;}
        string PhoneNumber { get;}
    }
}

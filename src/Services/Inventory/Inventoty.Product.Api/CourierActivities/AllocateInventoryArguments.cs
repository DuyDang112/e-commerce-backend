namespace Inventoty.Product.Api.CourierActivities
{
    public interface AllocateInventoryArguments
    {
        Guid OrderId { get; }
        List<OrderItems> Items { get; }
    }

    public interface OrderItems
    {
        string ProductNo { get; }
        int Quantity { get; }
    }
}

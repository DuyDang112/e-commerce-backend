namespace Payment.Api.CourierActivities
{
    public interface PaymentArguments
    {
        string OrderId { get;}
        string CardNumber { get;}
        string TotalAmount { get;}
    }
}

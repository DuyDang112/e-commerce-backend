namespace Basket.Api.Entities
{
    public class Cart
    {
        public string UserName { get; set; }

        public List<CartItem> Items { get; set; } = new();

        public Cart() { }
        public Cart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice => Items.Sum(x => x.ItemPrice * x.Quantity);

        public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.UtcNow;
        public string EmailAddress { get; set; }
        public string? JobId { get; set; }
    }
}

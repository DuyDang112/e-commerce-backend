using System.ComponentModel.DataAnnotations;

namespace Basket.Api.Entities
{
    public class CartItem
    {
        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage ="The Field {0} must be >= {1}.")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.5, double.PositiveInfinity, ErrorMessage = "The Field {0} must be >= {1}.")]
        public decimal ItemPrice { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public int AvailableQuantity { get;set; } 
        public void SetAvailableQuantity(int quantity) => AvailableQuantity = quantity;
    }
}

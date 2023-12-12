using Shared.Enums.Payment;
using System.ComponentModel.DataAnnotations;

namespace Basket.Api.Entities
{
    public class BasketCheckoutEntity
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PaymentCardNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ShipmentAddress { get; set; }
        //public EPaymentMethod PaymentMethod { get; set; } = EPaymentMethod.PaymentOnDelivery;
    }
}

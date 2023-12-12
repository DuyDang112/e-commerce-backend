using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using MassTransit;
using Shared.Enums.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction
{
    public record SubmitOrder : IntergrationBaseEvent, ISubmitOrder
    {
        //EPaymentMethod PaymentMethod { get; set; }
        //public Guid OrderId { get; set; }

        public DateTime Timestamp { get; set; }

        public string UserName { get; set; }

        public string PaymentCardNumber { get; set; }

        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ShipmentAddress { get; set; }
        public List<OrderItem> Items { get ; set ; } = new List<OrderItem>();

        public class OrderItem
        {
            public string ProductNo { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }

 
}

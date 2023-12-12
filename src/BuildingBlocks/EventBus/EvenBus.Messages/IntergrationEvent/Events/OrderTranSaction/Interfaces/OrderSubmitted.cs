using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces
{
    public interface OrderSubmitted
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }

        string UserName { get; }
        string PaymentCardNumber { get; }
        decimal TotalAmount { get; }
        string ItemNo { get; }
        decimal Quantity { get; }
        string ShipmentAddress { get; }
        string PhoneNumber { get; }

        List<OrderItem> OrdersItem{ get; }

        public interface OrderItem
        {
            string ProductNo { get; }
            int Quantity { get; }
        }
    }


}

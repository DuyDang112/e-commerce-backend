using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces
{
    public interface FulfillOrder
    {
        Guid OrderId { get; }

        string UserName { get; } 
        string PaymentCardNumber { get; }
        decimal TotalAmount { get; }
        string ShipmentAddress { get; }
        string PhoneNumber { get; }
        List<OrdersItem> Items { get; }
    }

    public interface OrdersItem
    {
        string ProductNo { get; }
        int Quantity { get; }
    }
}

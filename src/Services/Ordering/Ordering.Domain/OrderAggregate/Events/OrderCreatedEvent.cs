using Contract.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.OrderAggregate.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public long Id { get;private set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string DocumentNo { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }

        public OrderCreatedEvent(long id,string userName, decimal totalPrice, string documentNo, string emailAddress,string shippingAddress, string invoiceAddress)
        {
            Id = id;
            UserName = userName;
            TotalPrice = totalPrice;
            DocumentNo = documentNo;
            EmailAddress = emailAddress;
            ShippingAddress = shippingAddress;
            InvoiceAddress = invoiceAddress;
        }
    }
}

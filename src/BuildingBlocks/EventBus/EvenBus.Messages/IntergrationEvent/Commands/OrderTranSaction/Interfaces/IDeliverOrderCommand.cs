using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces
{
    public interface IDeliverOrderCommand : IIntergrationBaseEvent
    {
        Guid CorrelationId { get; }
        string DocumentNo { get; }
        string ShippingAddress { get; }
        string InvoiceAddress { get; }
    }
}

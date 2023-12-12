using Contract.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces
{
    public interface IOderRequestCommand : IIntergrationBaseEvent, IBaseCommandMediaR<long>
    {
        Guid CorrelationId { get; }
        string UserName { get; }
        decimal TotalPrice { get; }
        string FirstName { get; }
        string LastName { get; }
        string EmailAddress { get; }

        string ShippingAddress { get; }
        string InvoiceAddress { get; }
    }
}

using Shared.Enums.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces
{
    public interface IExecutePaymentCommand : IIntergrationBaseEvent
    {
        Guid CorrelationId { get; }
        string UserName { get; }
        decimal TotalPrice { get; }
        EPaymentMethod PaymentMethod { get; }
    }
}

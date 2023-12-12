using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces
{
    public interface IOrderSubmissionRejected
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }

        string UserName { get; }
        string Reason { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces
{
    public interface AllocationReleaseRequested
    {
        Guid AllocationId { get; }

        string Reason { get; }
    }
}

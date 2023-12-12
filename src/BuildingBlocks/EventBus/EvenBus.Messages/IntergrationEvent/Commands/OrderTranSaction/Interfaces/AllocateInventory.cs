using Shared.Enums.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces
{
    public interface AllocateInventory
    {
        Guid AllocationId { get; }
        string ItemNo { get; }
        int Quantity { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums.Shipment
{
    public enum EShipmentStatus
    {
        Ordered = 0,
        Processing = 1,
        Confirmed = 2,
        Packed = 3,
        In_Transit = 4,
        Arrived = 5,
        Delivered = 6,
        Canceled = 7,
        Pending_Confirmation = 8,
        Error = 9,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Messages
{
    public record IntergrationBaseEvent() : IIntergrationBaseEvent
    {
        public DateTime CreationTime { get; } = DateTime.Now;
        public Guid Id { get; set; }
    }
}

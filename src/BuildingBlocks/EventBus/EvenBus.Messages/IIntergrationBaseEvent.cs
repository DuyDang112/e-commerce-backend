using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Shared.SeedWork;

namespace EvenBus.Messages
{
    [ExcludeFromTopology]
    public interface IIntergrationBaseEvent 
    {
        DateTime CreationTime { get; }
        Guid Id { get; set; }
    }
}

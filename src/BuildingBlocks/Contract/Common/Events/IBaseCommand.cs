using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Common.Events
{
    [ExcludeFromTopology]
    public interface IBaseCommand<T> : IRequest<T>
    {
    }
}

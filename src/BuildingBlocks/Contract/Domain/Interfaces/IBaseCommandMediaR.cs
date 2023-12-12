using Amazon.Runtime.Internal;
using MediatR;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Domain.Interfaces
{
    public  interface IBaseCommandMediaR<T> : IRequest<ApiResult<T>>
    {
    }
}

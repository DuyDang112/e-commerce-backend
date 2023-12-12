using MediatR;
using Ordering.Application.Common.Model;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class GetOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        public string UserName { get; set; }

        public GetOrdersQuery(string username)
        {
            UserName = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}

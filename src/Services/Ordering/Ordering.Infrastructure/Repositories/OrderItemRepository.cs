using Contract.Common.Interfaces;
using Infrastructures.Common;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderItemRepository : RepositoryBaseAsync<OrderItem, long, OrderContext>, IOrderItemRepository
    {
        public OrderItemRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public  async Task<IList<long>> CreateOrdersItem(List<OrderItem> items) =>
            CreateList(items);
    }
}

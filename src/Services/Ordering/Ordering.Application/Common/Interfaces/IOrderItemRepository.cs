﻿using Contract.Common.Interfaces;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderItemRepository : IRepositoryBaseAsync<OrderItem, long>
    {
        Task<IList<long>> CreateOrdersItem(List<OrderItem> items);
    }
}

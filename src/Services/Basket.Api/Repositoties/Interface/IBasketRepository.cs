﻿using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Repositoties.Interface
{
    public interface IBasketRepository
    {
        Task<Cart> GetBasketByUserName (string userName);
        Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null);
        Task<bool> DeleteBasketFromUserName(string userName);
    }
}

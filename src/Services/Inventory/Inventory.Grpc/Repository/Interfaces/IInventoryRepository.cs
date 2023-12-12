﻿using Contract.Common.Interfaces;
using Inventory.Grpc.Entities;

namespace Inventory.Grpc.Repository.Interfaces
{
    public interface IInventoryRepository : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<int> GetStockQuantity(string itemNo);
    }
}

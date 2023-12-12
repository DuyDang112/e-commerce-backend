using Infrastructures.Common;
using Inventory.Grpc.Entities;
using Inventory.Grpc.Repository.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Repository
{
    public class InventoryRepository : MongoDbRepositoryBase<InventoryEntry>, IInventoryRepository
    {
        public InventoryRepository(IMongoClient mongoClient, MongoDbSettings mongoDbSettings) : base(mongoClient, mongoDbSettings)
        {
        }

        public async Task<int> GetStockQuantity(string itemNo) =>
            Collection.AsQueryable()
            .Where(x => x.ItemNo.Equals(itemNo))
            .Sum(x => x.Quantity);
    }
}

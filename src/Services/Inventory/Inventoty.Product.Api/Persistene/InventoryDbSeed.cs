using Inventoty.Product.Api.Entities;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventoty.Product.Api.Persistene
{
    public class InventoryDbSeed
    {
        public async Task SeedDataAsync(IMongoClient client, MongoDbSettings mongoDbSettings)
        {
            var databaseName = mongoDbSettings.DatabaseName;
            var database = client.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventotyEntry>("InventotyEntries");
            if(await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(getPreConfiguredInventoryEntries());
            }
        }

        private IEnumerable<InventotyEntry> getPreConfiguredInventoryEntries()
        {
            return new List<InventotyEntry>
            {
                new()
                {
                    Quantity = 10,
                    ItemNo = "meal"
                },
                new()
                {
                    Quantity = 20,
                    ItemNo = "drink"
                },
                new()
                {
                    Quantity = 20,
                    ItemNo = "lotus"
                },
                new()
                {
                    Quantity = 20,
                    ItemNo = "cateri"
                }
            };
        }
    }
}

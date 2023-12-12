using Contract.Common.Interfaces;
using Inventoty.Product.Api.Entities;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventoty.Product.Api.Services.Interfaces
{
    public interface IInventoryService : IMongoDbRepositoryBase<InventotyEntry>
    {
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        Task<PageList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);
        Task<InventoryEntryDto> GetByIdAsync(string id);
        Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
        Task<string> SaleItemAsync(string itemNo, int quantity);
        Task DeleteItemByDocumentNoAsync(string documentNo);
    }
}

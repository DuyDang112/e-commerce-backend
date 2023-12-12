using AutoMapper;
using Infrastructures.Common;
using Infrastructures.Extentions;
using Inventoty.Product.Api.Entities;
using Inventoty.Product.Api.Services.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.DTOs.Inventory;
using Shared.SeedWork;
using Shared.Enums.Inventory;

namespace Inventoty.Product.Api.Services
{
    public class InventoryService : MongoDbRepositoryBase<InventotyEntry>, IInventoryService
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient mongoClient, MongoDbSettings mongoDbSettings, IMapper mapper) : base(mongoClient, mongoDbSettings)
        {
            _mapper = mapper;
        }

        public async Task DeleteItemByDocumentNoAsync(string documentNo)
        {
            FilterDefinition<InventotyEntry> filter = Builders<InventotyEntry>.Filter.Eq(s => s.DocumentNo, documentNo);
             await Collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll()
                .Find(x => x.ItemNo.Equals(itemNo))
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);

            return result;
        }

        public async Task<PageList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchTerm = Builders<InventotyEntry>.Filter.Empty;
            var filterItemNo = Builders<InventotyEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());
            if (!string.IsNullOrEmpty(query.Searchterm))
                filterSearchTerm = Builders<InventotyEntry>.Filter.Eq(x => x.DocumentNo, query.Searchterm);

            var andFilter = filterItemNo & filterSearchTerm;
            //var pageList = await Collection.Find(andFilter)
            //    .Skip((query.PageNumber - 1) * query.PageSize)
            //    .Limit(query.PageSize)
            //    .ToListAsync();

            var pageList = await Collection.PaginatedListAsync(andFilter, 
                pageIndex: query.PageNumber, pageSize: query.PageSize);

            var items = _mapper.Map<IEnumerable<InventoryEntryDto>>(pageList);
            var result = new PageList<InventoryEntryDto>(items, pageList.GetMetaData().TotalItems,
                pageNumber: query.PageNumber, pageSize: query.PageSize);

            return result;
        }

        public async Task<InventoryEntryDto> GetByIdAsync(string id)
        {
            FilterDefinition<InventotyEntry> filter = Builders<InventotyEntry>.Filter.Eq(x => x.Id, id);
            var entity = await FindAll().Find(filter).FirstOrDefaultAsync();

            var result = _mapper.Map<InventoryEntryDto>(entity);
            return result;
        }

        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            var entity = new InventotyEntry()
            {
                ItemNo = model.GetItemNo(),
                Quantity = model.Quantity,
                DocumentType = model.DocumentType
            };

            await CreateAsync(entity);
            var result = _mapper.Map<InventoryEntryDto>(entity);

            return result;
        }

        public async Task<string> SaleItemAsync(string itemNo, int quantity)
        {
            var entity = new InventotyEntry()
            {
                ItemNo = itemNo,
                Quantity = quantity * -1,
                DocumentType = EDocumentType.Sale
            };

            await CreateAsync(entity);

            return entity.DocumentNo;
        }
    }
}

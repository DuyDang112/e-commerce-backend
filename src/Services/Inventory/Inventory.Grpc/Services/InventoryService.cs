using Grpc.Core;
using Inventory.Grpc.Protos;
using Inventory.Grpc.Repository.Interfaces;

namespace Inventory.Grpc.Services
{
    public class InventoryService : StockProtoService.StockProtoServiceBase
    {
        private readonly IInventoryRepository _repository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryRepository repository, ILogger<InventoryService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<StockModel> GetStock(GetStockRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"BEGIN Get Stock By ItemNo: {request.ItemNo}");
            var stockQuantity = await _repository.GetStockQuantity(request.ItemNo);
            var result = new StockModel()
            {
                Quantity = stockQuantity,
            };
            _logger.LogInformation($"END Get Stock By ItemNo: {request.ItemNo} - quantity: {result}");
            return result;
        }
    }
}

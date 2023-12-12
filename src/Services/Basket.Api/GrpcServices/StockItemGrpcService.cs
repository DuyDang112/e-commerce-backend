using Inventory.Grpc.Protos;
namespace Basket.Api.GrpcServices
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _grpcClient;
        private readonly ILogger<StockItemGrpcService> _logger;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient grpcClient,ILogger<StockItemGrpcService> logger)
        {
            _grpcClient = grpcClient
                ?? throw new ArgumentException(nameof(grpcClient));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                var stockItemModel = new GetStockRequest { ItemNo = itemNo };
                return await _grpcClient.GetStockAsync(stockItemModel);
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}

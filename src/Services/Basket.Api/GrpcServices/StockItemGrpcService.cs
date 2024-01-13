using Grpc.Core;
using Inventory.Grpc.Protos;
using Polly;
using Polly.Retry;
using ILogger = Serilog.ILogger;
namespace Basket.Api.GrpcServices
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _grpcClient;
        private readonly ILogger _logger;
        private readonly AsyncRetryPolicy<StockModel> _asyncRetryPolicy;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient grpcClient,ILogger logger)
        {
            _grpcClient = grpcClient
                ?? throw new ArgumentException(nameof(grpcClient));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _asyncRetryPolicy = Policy<StockModel>.Handle<RpcException>()
                .RetryAsync(3);
        }

        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                _logger.Information($"BEGIN: Get stock StockItemGrpcService ItemNo: {itemNo}");
                var stockItemModel = new GetStockRequest { ItemNo = itemNo };

                return await _asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    var result = await _grpcClient.GetStockAsync(stockItemModel);
                    if(result != null)
                        _logger.Information($"END: Get stock StockItemGrpcService ItemNo: {itemNo} - Quantity: {result.Quantity}");
                    return result;
                });
                
            }catch (Exception ex)
            {
                _logger.Error($"GRPC StockItemgRPCService Failed {ex.Message}");

                return new StockModel
                {
                    Quantity = -1
                };
            }
        }
    }
}

using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Inventoty.Product.Api.Services.Interfaces;
using MassTransit;
using MassTransit.Courier;
using Org.BouncyCastle.Utilities;
using ILogger = Serilog.ILogger;

namespace Inventoty.Product.Api.CourierActivities
{
    public class AllocateInventoryActivity : IActivity<AllocateInventoryArguments, AllocateInventoryLog>
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger _logger;

        public AllocateInventoryActivity(IInventoryService inventoryService,
            ILogger logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<AllocateInventoryArguments> context)
        {
            var orderId = context.Arguments.OrderId;

            var Items = context.Arguments.Items;

            if (Items == null)
                throw new ArgumentNullException(nameof(Items));

            var allocationIds = new List<string>();

            Items.ForEach(async x =>
            {
                string result = "";
                try
                {
                    _logger.Information($"Start Sale Item No: {x.ProductNo} - Quantity {x.Quantity}");
                    result = await _inventoryService.SaleItemAsync(x.ProductNo, x.Quantity);
                    allocationIds.Add(result);
                    _logger.Information($"End Sale Item No: {x.ProductNo} - Quantity {x.Quantity}");
                }
                catch(Exception ex)
                {
                    throw new Exception($"An Error has occurred while sale item with DocumentNo: {result} - {ex.Message}");
                }
            });

            //await context.Publish<InventoryAllocated>(new
            //{
            //    OrderId = orderId
            //});

            return context.Completed(new { AllocationIds = allocationIds });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<AllocateInventoryLog> context)
        {
            try
            {
                context.Log.AllocationIds.ToList().ForEach(async documentNo =>
                {
                    await _inventoryService.DeleteItemByDocumentNoAsync(documentNo);
                    _logger.Information($"AllocateInventory with Documentno: {documentNo} was deleted");
                });
                
                //await context.Publish<AllocationReleaseRequested>(new
                //{
                //    context.Log.AllocationIds,
                //    Reason = "Order Faulted"
                //});
            }
            catch (Exception ex)
            {
                _logger.Information($"An error was occured: {ex.Message}");
            }

            return context.Compensated();
        }

    }
}

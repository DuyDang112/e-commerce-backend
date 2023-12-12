using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Inventoty.Product.Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using Shared.SeedWork;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventoty.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IPublishEndpoint _publishEndpoint;

        public InventoryController(IInventoryService inventoryService,
            IPublishEndpoint publishEndpoint)
        {
            _inventoryService = inventoryService;
            _publishEndpoint = publishEndpoint;
        }

        [Route("items/{itemNo}",Name = "GetAllByItemNo")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
        {
            var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        [Route("items/{itemNo}/paging", Name = "GetAllByItemNoPaging")]
        [HttpGet]
        [ProducesResponseType(typeof(PageList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNoPaging([Required] string itemNo,
            [FromQuery] GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);
            var result = await _inventoryService.GetAllByItemNoPagingAsync(query);
            return Ok(result);
        }

        [Route("{id}", Name = "GetById")]
        [HttpGet]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> GetById([Required] string id)
        {
            var result = await _inventoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [Route("purchase/{itemNo}", Name = "PurchaseOrder")]
        [HttpPost]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string itemNo,
            [FromBody] PurchaseProductDto model)
        {
            model.SetItemNo(itemNo);
            var result = await _inventoryService.PurchaseItemAsync(itemNo, model);
            return Ok(result);
        }

        [Route("{id}", Name = "DeleteById")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteById([Required] string id)
        {
            var entity = await _inventoryService.GetByIdAsync(id);
            if (entity == null) return NotFound();

            await _inventoryService.DeleteAsync(id);
            return NoContent();
        }

        [Route("AllocateInventory", Name = "AllocateInventory")]
        [HttpPatch]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> AllocateInventory([Required] string OrderId)
        {
            await _publishEndpoint.Publish<InventoryAllocated>(new
            {
                OrderId = OrderId
            });
            return Accepted();
        }

    }
}

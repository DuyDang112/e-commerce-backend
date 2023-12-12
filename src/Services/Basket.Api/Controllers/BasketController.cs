using AutoMapper;
using Basket.Api;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositoties.Interface;
using Contract.Services;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Google.Protobuf.WellKnownTypes;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ISmtpEmailService _smtpEmailService;
        private readonly IMapper _mapper;
        private readonly IRequestClient<SubmitOrder> _submidOrderRequest;
        private readonly StockItemGrpcService _stockGrpcService;
        private readonly EventBusSettings _eventBusSettings;

        public BasketController(IBasketRepository basketRepository, 
            IMapper mapper, 
            IPublishEndpoint publishEndpoint,
            StockItemGrpcService stockItemGrpcService,
            EventBusSettings eventBusSettings,
            IRequestClient<SubmitOrder> submidOrderRequest)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _submidOrderRequest = submidOrderRequest;
            _stockGrpcService = stockItemGrpcService;
            _eventBusSettings = eventBusSettings;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string username)
        {
            var result = await _basketRepository.GetBasketByUserName(username);
            return Ok(result ?? new Cart());
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
        {
            // Communicate with Inventory.Grpc
            foreach (var item in cart.Items)
            {
                var stock = await _stockGrpcService.GetStock(item.ItemNo);
                item.SetAvailableQuantity(stock.Quantity);
            }
            var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // monitor after each 5 minutes

            var result = await _basketRepository.UpdateBasket(cart, options);
            return Ok(result);
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)] // display type return in swagger
        public async Task<ActionResult<bool>> DeleteBasket([Required]string username)
        {
            var result = await _basketRepository.DeleteBasketFromUserName(username);
            return Ok(result);
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutEntity basketCheckout)
        {
            var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);
            if (basket == null) return NotFound();

            var eventMessage = _mapper.Map<SubmitOrder>(basketCheckout);
            foreach (var item in basket.Items)
            {
                var orderItem = new SubmitOrder.OrderItem
                {
                    ProductNo = item.ItemNo,
                    ProductName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.ItemPrice
                };
                eventMessage.Items.Add(orderItem);
            }
            eventMessage.Timestamp = DateTime.UtcNow;
            eventMessage.TotalAmount = basket.TotalPrice;
            eventMessage.EmailAddress ??= basket.EmailAddress;
            

            var (accepted, rejected) = await _submidOrderRequest.GetResponse<IOrderSubmissionAccepted, IOrderSubmissionRejected>(eventMessage);

            // Return true if no has exceptions happen
            if (accepted.IsCompletedSuccessfully)
            {
                await _basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);

                var response = await accepted;

                return Accepted(response);
            }

            //return true when task completed whether failure or successly
            if (accepted.IsCompleted)
            {
                await accepted;

                return Problem("Order was not accepted");
            }
            else
            {
                var response = await rejected;

                return BadRequest(response.Message);
            }
        }
    }
}

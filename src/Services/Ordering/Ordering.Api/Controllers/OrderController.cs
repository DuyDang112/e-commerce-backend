using Contract.Services;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Model;
using Ordering.Application.Features.V1.Orders;
using Ordering.Domain.Enums;
using Shared.SeedWork;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ISmtpEmailService _smtpEmailService;
        readonly IPublishEndpoint _publishEndpoint;
        readonly IRequestClient<CheckOrder> _checkOrderClient;
        public OrderController(IMediator mediator, ISmtpEmailService smtpEmailService,
            IPublishEndpoint publishEndpoint,
            IRequestClient<CheckOrder> checkOrderClient
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _smtpEmailService = smtpEmailService;
            _publishEndpoint = publishEndpoint;
            _checkOrderClient = checkOrderClient;
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string GetStatusOrder = nameof(GetStatusOrder);
            public const string AcceptOrder = nameof(AcceptOrder);
            public const string CreateOrder = nameof(CreateOrder);
            public const string UpdateOrder = nameof(UpdateOrder);
            public const string DeleteOrder = nameof(DeleteOrder);
        }

        [HttpGet("{username}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        // note ActionResult not IActionResult
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string username)
        {
            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch("{OrderId}", Name = RouteNames.AcceptOrder)]
        [ProducesResponseType(typeof(AcceptedResult), (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Patch(Guid OrderId)
        {
            await _publishEndpoint.Publish<OrderAccepted>(new
            {
                OrderId = OrderId,
                InVar.Timestamp,
            });

            return Accepted();
        }

        [HttpGet("GetStatusOrder/{OrderId}", Name = RouteNames.GetStatusOrder)]
        [ProducesResponseType(typeof(AcceptedResult), (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Get(Guid OrderId)
        {
            var (status, notFound) = await _checkOrderClient.GetResponse<OrderStatus, OrderNotFound>(new { OrderId = OrderId });

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }


        }
    }
}

using MediatR;
using Ordering.Domain.OrderAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Ordering.Application.Features.V1.Orders.EventHandlers
{
    public class OrdersDomainHandler : INotificationHandler<OrderCreatedEvent>,
        INotificationHandler<OrderDeletedEvent>
    {
        private readonly ILogger _logger;

        public OrdersDomainHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information($"Ordering Domain Event: {notification.GetType().Name} ");
            return Task.CompletedTask;
        }

        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.Information($"Ordering Domain Event: {notification.GetType().Name} ");
            return Task.CompletedTask;
        }
    }
}

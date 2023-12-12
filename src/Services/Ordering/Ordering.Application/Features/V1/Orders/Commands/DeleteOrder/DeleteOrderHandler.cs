using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Ordering.Application.Common.Exceptions;

namespace Ordering.Application.Features.V1.Orders
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public DeleteOrderHandler(IOrderRepository orderRepository, ILogger logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }



        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if (orderEntity == null) throw new NotFoundException(nameof(orderEntity), request.Id);

            _orderRepository.Delete(orderEntity);
            //publish event deleted
            orderEntity.DeletedOrder();
            await _orderRepository.SaveChangesAsync();

            _logger.Information($"Order {orderEntity.Id} was successfully deleted");

            return Unit.Value;
        }

    }
}

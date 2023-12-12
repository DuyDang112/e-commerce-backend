using AutoMapper;
using Contract.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Model;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Ordering.Domain.Entities;
using Ordering.Application.Common.Exceptions;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand,ApiResult<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateOrderHandler(IOrderRepository orderRepository, IMapper mapper, ILogger logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        private const string MethodName = "UpdateOrderCommandHandler";

        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if (orderEntity == null) throw new NotFoundException(nameof(Order), request.Id);

            _logger.Information($"BEGIN: {MethodName}");

            orderEntity = _mapper.Map(request, orderEntity);
            var updateOrder = await _orderRepository.UpdateOrderAsync(orderEntity);
            await _orderRepository.SaveChangesAsync();
            _logger.Information($"Order {request.Id} is successfully updated");
            var result = _mapper.Map<OrderDto>(updateOrder);

            _logger.Information($"END: {MethodName}");

            return new ApiSuccessResult<OrderDto>(result);
        }
    }
}

using AutoMapper;
using Contract.Services;
using Ordering.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Shared.SeedWork;
using Ordering.Domain.Entities;
using Shared.Services.Email;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using MassTransit;
using OrderItem = Ordering.Domain.Entities.OrderItem;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        private readonly ISmtpEmailService _smtpEmailService;
        private readonly ILogger _logger;

        public CreateOrderHandler(IOrderRepository orderRepository,
            IMapper mapper,
            ISmtpEmailService smtpEmailService,
            ILogger logger,
            IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _smtpEmailService = smtpEmailService ?? throw new ArgumentNullException(nameof(smtpEmailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderItemRepository = orderItemRepository ?? throw new ArgumentNullException(nameof(orderItemRepository));
        }

        private const string MethodName = "CreateOrderCommandHandler";

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
                var Order = new Order
                {
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ShippingAddress = request.ShipmentAddress,
                    EmailAddress = request.EmailAddress,
                    //PhoneNumber = request.PhoneNumber,
                    TotalPrice = request.TotalAmount,
                    InvoiceAddress = "invoiceAddress"
                };

                _orderRepository.Create(Order);

                //publish event addedOrder
                Order.AddedOrder();
                await _orderRepository.SaveChangesAsync();

                var ordersItem = new List<Ordering.Domain.Entities.OrderItem>();
                request.Items.ForEach(item =>
                {
                    ordersItem.Add(new Ordering.Domain.Entities.OrderItem
                    {
                        ProductNo = item.ProductNo,
                        OrderId = Order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                });

                await _orderItemRepository.CreateOrdersItem(ordersItem);



                SendEmailAsync(Order, cancellationToken);

                _logger.Information($"Order {Order.Id} is successfully created");

                _logger.Information($"END: {MethodName}");

                return Order.DocumentNo;
            
        }

        private async void SendEmailAsync(Order order, CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = order.EmailAddress,
                Body = "Order Was created",
                Subject = "Order was created"
            };

            try
            {
                await _smtpEmailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent Created order to email {order.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {order.Id} failed due to an error with the email service: {ex.Message}");
            }
        }
    }
}

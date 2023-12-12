using AutoMapper;
using Contract.Services;
using EvenBus.Messages.Abstractions;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;
using Infrastructures.Services;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Features.V1.Orders;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Repositories;
using Shared.SeedWork;
using Shared.Services.Email;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading;
using ILogger = Serilog.ILogger;

namespace Ordering.Api.Aplication.IntergrationEvents.CommandsHandler
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public SubmitOrderConsumer(ISender sender, ILogger logger, IMapper mapper)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private const string consumerName = "Submit-Order-Consumer";
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            _logger.Information($"BEGIN: {consumerName}");
            var DocumentNo = Guid.Empty;
            try
            {
                var command = _mapper.Map<CreateOrderCommand>(context.Message);
                DocumentNo = await _sender.Send(command);
                _logger.Information($"SubmitOrder Sent with DocumentNo: {DocumentNo}");

                var OrdersItem = context.Message.Items.ToList().Select(item => new
                {
                    item.ProductNo,
                    item.Quantity
                }).ToList();
                
                await context.Publish<OrderSubmitted>(new
                {
                    OrderId = DocumentNo,
                    context.Message.Timestamp,
                    context.Message.UserName,
                    context.Message.PaymentCardNumber,
                    context.Message.TotalAmount,
                    context.Message.ShipmentAddress,
                    context.Message.PhoneNumber,
                    OrdersItem,
                });

                if (context.RequestId != null)
                    await context.RespondAsync<IOrderSubmissionAccepted>(new
                    {
                        InVar.Timestamp,
                        OrderId = DocumentNo,
                        context.Message.UserName
                    });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error was happened while create order: {ex.Message}");
                if (context.RequestId != null)
                    await context.RespondAsync<IOrderSubmissionRejected>(new
                    {
                        InVar.Timestamp,
                        OrderID = DocumentNo,
                        context.Message.UserName,
                        Reason = $@"An error was happened: {ex.Message}
                                cannot submit orders: {DocumentNo}
                                UserName: {context.Message.UserName}"
                    });

                return;
            }
            
            _logger.Information($"END: {consumerName}");
        }   
    }
}

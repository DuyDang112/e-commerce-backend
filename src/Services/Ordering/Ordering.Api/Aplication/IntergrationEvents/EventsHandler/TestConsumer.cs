//using AutoMapper;
//using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
//using EvenBus.Messages.IntergrationEvent.Events;
//using MassTransit;
//using MediatR;
//using Ordering.Application.Features.V1.Orders;
//using ILogger = Serilog.ILogger;

//namespace Ordering.Api.Aplication.IntergrationEvents.EventsHandler
//{
//    public class TestConsumer : IConsumer<OderRequestCommand>
//    {
//        private readonly IMediator _mediator;
//        private readonly IMapper _mapper;
//        private readonly ILogger _logger;

//        public TestConsumer(IMediator mediator, IMapper mapper, ILogger logger)
//        {
//            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
//            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        public async Task Consume(ConsumeContext<OderRequestCommand> context)
//        {
//            var command = _mapper.Map<CreateOrderCommand>(context.Message);
//            var result = await _mediator.Send(command);
//            //_logger.Information("BasketCheckoutEvent consumed succsessfully." +
//            //    "Order is created with ID: {newOrderId}", result.Data);
//        }
//    }
//}

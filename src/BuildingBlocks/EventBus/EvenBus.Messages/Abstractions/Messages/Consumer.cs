using MassTransit;
using MediatR;

namespace EvenBus.Messages.Abstractions
{ 
    public abstract class Consumer<TMessage> : IConsumer<TMessage>
        where TMessage : class, IIntergrationBaseEvent
    {
        private readonly ISender _sender;

        protected Consumer(ISender sender)
        {
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<TMessage> context)
            => await _sender.Send(context.Message);
    }
}

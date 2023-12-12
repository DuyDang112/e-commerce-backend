using AutoMapper;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Events;
using Ordering.Domain.Entities;

namespace Ordering.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<OderRequestCommand, Order>();
        }
    }
}

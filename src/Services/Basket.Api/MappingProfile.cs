using AutoMapper;
using Basket.Api.Entities;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Events.OrderTranSaction.Interfaces;

namespace Basket.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckoutEntity, SubmitOrder>();
        }
    }
}

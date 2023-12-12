using AutoMapper;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Events;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrderCommand : CreateOrUpdateCommand, IRequest<Guid>, IMapFrom<SubmitOrder>
    {
        public string UserName { get; set; }
        public void Mapping(Profile profile)
        {
            //profile.CreateMap<CreateOrderCommand, Order>();
            profile.CreateMap<SubmitOrder, CreateOrderCommand>();
        }
    }


}

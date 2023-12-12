using AutoMapper;
using Infrastructures.Mapping;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Model;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
    {
        public long Id { get; set; }

        public void SetId(long id)
        {
            Id = id;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.Status, otps => otps.Ignore())
                .IgnoreAllNonExisting();
        }
    }
}

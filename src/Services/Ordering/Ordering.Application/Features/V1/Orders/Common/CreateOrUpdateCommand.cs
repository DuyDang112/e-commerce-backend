using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using AutoMapper;
using OrderItemDto = EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.SubmitOrder.OrderItem;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrUpdateCommand : IMapFrom<SubmitOrder>
    {
        public Guid OrderId { get; set; }

        public DateTime Timestamp { get; set; }

        public string PaymentCardNumber { get; set; }

        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ShipmentAddress { get; set; }
        public List<OrderItem> Items { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SubmitOrder, CreateOrUpdateCommand>();
        }

        public class OrderItem : IMapFrom<OrderItemDto>
        {
            public void Mapping(Profile profile)
            {
                profile.CreateMap<OrderItemDto, OrderItem>();
            }
            public string ProductNo { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }

 
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Model
{
    public class OrderDto : IMapFrom<Order>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }

        public EOrderStatus Status { get; set; }
    }
}

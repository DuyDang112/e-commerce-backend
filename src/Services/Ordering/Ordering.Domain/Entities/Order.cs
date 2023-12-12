using Contract.Common.Events;
using Contract.Domain;
using Ordering.Domain.Enums;
using Ordering.Domain.OrderAggregate.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order : AuditableEventEntity<long>
    {
        [Required]
        public string UserName { get; set; }
        [Column(TypeName = "uniqueidentifier")]
        public Guid DocumentNo { get; set; } = Guid.NewGuid();
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string ShippingAddress { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string InvoiceAddress { get; set; }

        public EOrderStatus Status { get; set; }

        public List<OrderItem> Items { get; set; }

        public Order AddedOrder()
        {
            AddDomainEvent(new OrderCreatedEvent(Id, UserName, TotalPrice, DocumentNo.ToString(), EmailAddress, ShippingAddress, InvoiceAddress));
            return this;
        }
        public Order DeletedOrder()
        {
            AddDomainEvent(new OrderDeletedEvent(Id));
            return this;
        }
    }
}

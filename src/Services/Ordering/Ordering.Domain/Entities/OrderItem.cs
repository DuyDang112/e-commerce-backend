using Contract.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class OrderItem : EntityAuditBase<long>
    {
        public long OrderId { get; set; }
        public string ProductNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}

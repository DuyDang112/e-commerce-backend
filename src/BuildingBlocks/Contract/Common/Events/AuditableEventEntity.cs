using Contract.Domain;
using Contract.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Common.Events
{
    public abstract class AuditableEventEntity<T> :  EventEntity<T>, IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LassModifiedDate { get; set; }

    }
}

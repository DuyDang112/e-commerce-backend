using Contract.Common.Events;
using Contract.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Common.Interfaces
{
    public interface IEventEntity
    {
        void AddDomainEvent(BaseEvent domainEvent);
        void RemoveDomainEvent(BaseEvent domainEvent);
        void ClearDomainEvents();
        IReadOnlyCollection<BaseEvent> GetDomainEvents();
    }

    public interface IEventEntity<T> : IEntityBase<T>, IEventEntity
    {

    }
}

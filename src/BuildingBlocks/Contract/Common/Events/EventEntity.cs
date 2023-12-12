﻿using Contract.Common.Interfaces;
using Contract.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Common.Events
{
    public class EventEntity<T> : EntityBase<T>, IEventEntity<T>
    {
        private readonly List<BaseEvent> _domainEvents = new();
        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public IReadOnlyCollection<BaseEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Events;

namespace xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Aggregate
{
    public class AggregateRoot<TIdentityType> : IAggregateRoot<TIdentityType>
    {
        [JsonProperty(PropertyName = "id")]
        public TIdentityType Id { get; set; }

        List<IDomainEvent> uncommittedEvents = new();

        protected void Emit(IDomainEvent domainEvent)
        {
            uncommittedEvents.Add(domainEvent);
        }
    }
}

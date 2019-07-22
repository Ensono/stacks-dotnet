using System.Collections.Generic;
using Amido.Stacks.Domain.Events;

namespace Amido.Stacks.Domain
{
    public class AggregateRoot : IAggregateRoot
    {
        //TODO: Add state machine for state transition

        List<IDomainEvent> uncommittedEvents = new List<IDomainEvent>();

        protected void Emit(IDomainEvent domainEvent)
        {
            uncommittedEvents.Add(domainEvent);
            //RaiseEvent(domainEvent);
        }

        //TODO: Decide how link Events to Handlers
        // https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1003-use-generic-event-handler-instances?view=vs-2019
        //private void RaiseEvent(IDomainEvent domainEvent)
        //{
        //    On?.Invoke(this, domainEvent);
        //}

        //public event EventHandler<IDomainEvent> On;
    }
}

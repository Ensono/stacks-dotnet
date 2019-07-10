using System;
using Amido.Stacks.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class MenuUpdatedEvent : IApplicationEvent
    {
        public MenuUpdatedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}

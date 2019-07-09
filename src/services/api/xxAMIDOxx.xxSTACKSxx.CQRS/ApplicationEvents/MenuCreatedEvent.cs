using System;
using Amido.Stacks.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class MenuCreatedEvent : IApplicationEvent
    {
        public MenuCreatedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}

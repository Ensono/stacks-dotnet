using System;
using Amido.Stacks.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class MenuDeletedEvent : IApplicationEvent
    {
        public MenuDeletedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}

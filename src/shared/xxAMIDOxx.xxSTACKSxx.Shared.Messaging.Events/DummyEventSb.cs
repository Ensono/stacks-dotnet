using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Events
{
    public class DummyEventSb : IApplicationEvent
    {
        public DummyEventSb() { }

        public DummyEventSb(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }

        public int EventCode => 9871;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }
    }
}

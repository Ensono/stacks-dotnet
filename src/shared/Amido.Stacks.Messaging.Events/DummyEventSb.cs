using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace Amido.Stacks.Messaging.Events
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

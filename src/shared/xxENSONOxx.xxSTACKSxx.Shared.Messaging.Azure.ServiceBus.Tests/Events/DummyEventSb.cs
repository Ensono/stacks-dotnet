using System;
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events;

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

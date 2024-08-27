using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Tests.Serializers
{
    [method: JsonConstructor]
    public class DummyEventAes(int operationCode, Guid correlationId) : IApplicationEvent
    {
        public DummyEventAes(IOperationContext context) : this(context.OperationCode, context.CorrelationId)
        {
        }

        public int EventCode => 9871;

        public int OperationCode { get; } = operationCode;

        public Guid CorrelationId { get; } = correlationId;
    }
}

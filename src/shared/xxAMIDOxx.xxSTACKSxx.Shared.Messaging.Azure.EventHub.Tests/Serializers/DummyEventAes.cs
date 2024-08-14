using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Tests.Serializers
{
    public class DummyEventAes : IApplicationEvent
    {
        [JsonConstructor]
        public DummyEventAes(int operationCode, Guid correlationId)
        {
            OperationCode = operationCode;
            CorrelationId = correlationId;
        }

        public DummyEventAes(IOperationContext context)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
        }

        public int EventCode => 9871;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }
    }
}

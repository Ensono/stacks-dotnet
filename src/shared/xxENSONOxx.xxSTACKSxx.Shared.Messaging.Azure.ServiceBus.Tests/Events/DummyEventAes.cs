using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events
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

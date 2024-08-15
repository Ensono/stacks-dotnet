using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Events
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

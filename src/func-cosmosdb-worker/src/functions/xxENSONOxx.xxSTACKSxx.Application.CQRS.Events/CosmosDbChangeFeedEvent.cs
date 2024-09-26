using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Operations;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events
{
    [method: JsonConstructor]
    public class CosmosDbChangeFeedEvent(int operationCode, Guid correlationId, Guid entityId, string eTag)
        : IApplicationEvent
    {
        public CosmosDbChangeFeedEvent(IOperationContext context, Guid entityId, string eTag) : this(context.OperationCode, context.CorrelationId, entityId, eTag)
        {
        }

		public int EventCode => (int)Enums.EventCode.EntityUpdated;

		public int OperationCode { get; private set; } = operationCode;

        public Guid CorrelationId { get; private set; } = correlationId;

        public Guid EntityId { get; private set; } = entityId;

        public string ETag { get; private set; } = eTag;
    }
}

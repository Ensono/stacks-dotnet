#if CosmosDb
using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;

[method: JsonConstructor]
public class CosmosDbChangeFeedEvent(int operationCode, Guid correlationId, Guid entityId, string eTag)
    : IApplicationEvent
{
    public CosmosDbChangeFeedEvent(IOperationContext context, Guid entityId, string eTag) : this(context.OperationCode, context.CorrelationId, entityId, eTag)
    {
    }

    public int EventCode => (int)Enums.EventCode.EntityUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid EntityId { get; } = entityId;

    public string ETag { get; } = eTag;
}
#endif

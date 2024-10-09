using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.Worker;

[method: JsonConstructor]
public sealed class CosmosDbChangeFeedEvent(int operationCode, Guid correlationId, Guid entityId, string eTag)
                  : IApplicationEvent
{
    public CosmosDbChangeFeedEvent(IOperationContext context, Guid entityId, string eTag)
         : this(context.OperationCode, context.CorrelationId, entityId, eTag)
    {
    }

    public int EventCode => (int)Worker.EventCode.EntityUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid EntityId { get; } = entityId;

    public string ETag { get; } = eTag;
}

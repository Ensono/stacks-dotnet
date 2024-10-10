//using Newtonsoft.Json;

using System.Text.Json.Serialization;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.Worker;


public class CosmosDbChangeFeedEvent : IApplicationEvent
{
    [JsonConstructor]
    public CosmosDbChangeFeedEvent(int operationCode, Guid correlationId, Guid entityId, string eTag)
    {
        OperationCode = operationCode;
        CorrelationId = correlationId;
        EntityId = entityId;
        ETag = eTag;
    }

    public CosmosDbChangeFeedEvent(IOperationContext context, Guid entityId, string eTag)
         : this(context.OperationCode, context.CorrelationId, entityId, eTag)
    {
    }


    public int EventCode => (int)Worker.EventCode.EntityUpdated;

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public Guid EntityId { get; }

    public string ETag { get; }
}

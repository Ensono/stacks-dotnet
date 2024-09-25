using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Worker;

public class CosmosDbChangeFeedListener(
    IApplicationEventPublisher appEventPublisher,
    ILogger<CosmosDbChangeFeedListener> logger)
{
    [Function(Constants.FunctionNames.CosmosDbChangeFeedListener)]
    public void Run([CosmosDBTrigger(
        databaseName: "%COSMOSDB_DATABASE_NAME%",
        containerName: "%COSMOSDB_CONTAINER_NAME%",
        Connection = "COSMOSDB_CONNECTION_STRING",
        LeaseContainerName = "%COSMOSDB_LEASE_COLLECTION_NAME%",
        CreateLeaseContainerIfNotExists  = true)]IReadOnlyList<CosmosDbChangeFeedEvent>? input)
    {
        if (input is { Count: > 0 })
        {
            logger.LogInformation("Documents modified " + input.Count);
            foreach (var changedItem in input)
            {
                logger.LogInformation("Document read. Id: " + changedItem.EntityId);

                var cosmosDbEvent = new CosmosDbChangeFeedEvent(
                    operationCode: changedItem.OperationCode,
                    correlationId: changedItem.CorrelationId,
                    entityId: changedItem.EntityId,
                    eTag: changedItem.ETag);

                appEventPublisher.PublishAsync(cosmosDbEvent);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Worker;

public class ChangeFeedListener(
    IApplicationEventPublisher appEventPublisher,
    ILogger<ChangeFeedListener> logger)
{
    [FunctionName(Constants.FunctionNames.CosmosDbChangeFeedListener)]
    public void Run([CosmosDBTrigger(
        databaseName: "%COSMOSDB_DATABASE_NAME%",
        containerName: "%COSMOSDB_COLLECTION_NAME%",
        Connection  = "COSMOSDB_CONNECTIONSTRING",
        LeaseContainerName  = "%COSMOSDB_LEASE_COLLECTION_NAME%",
        CreateLeaseContainerIfNotExists  = true)]IReadOnlyList<CosmosDbChangeFeedEvent> input)
    {
        if (input != null && input.Count > 0)
        {
            logger.LogInformation("Documents modified " + input.Count);
            foreach (var changedItem in input)
            {
                logger.LogInformation("Document read. Id: " + changedItem.EntityId);

                var cosmosDbEvent = new CosmosDbChangeFeedEvent(
                    operationCode: 999,
                    correlationId: Guid.NewGuid(),
                    entityId: changedItem.EntityId,
                    eTag: changedItem.ETag);

                appEventPublisher.PublishAsync(cosmosDbEvent);
            }
        }
    }
}

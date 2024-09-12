#if CosmosDb
using System;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Exceptions;

namespace xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;

public abstract class CosmosDBException : InfrastructureExceptionBase
{
    public CosmosDBException(
        int exceptionCode,
        string message,
        string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
        Exception innerException
    ) : base(message, innerException)
    {
        DatabaseAccountUri = databaseAccountUri;
        DatabaseName = databaseName;
        ContainerName = containerName;
        PartitionKey = partitionKey;
        ItemId = itemId;
        ExceptionCode = exceptionCode;

        Data["DatabaseAccountUri"] = DatabaseAccountUri;
        Data["DatabaseName"] = DatabaseName;
        Data["ContainerName"] = ContainerName;
        Data["PartitionKey"] = PartitionKey;
        Data["ItemId"] = ItemId;
    }

    public override int ExceptionCode { get; protected set; }

    public string DatabaseAccountUri { get; }
    public string DatabaseName { get; }
    public string ContainerName { get; }
    public string PartitionKey { get; }
    public string ItemId { get; }
}
#endif

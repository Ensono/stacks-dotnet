using System;

namespace xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;

public class DocumentHasChangedException : CosmosDBException
{
    private DocumentHasChangedException(
        int exceptionCode,
        string message,
        string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, string eTag,
        Exception innerException
    ) : base(exceptionCode, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
    {
        ETag = eTag;

        Data["ETag"] = ETag;
    }

    public string ETag { get; }

    public static void Raise(string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, string eTag, Exception exception = null)
    {
        throw new DocumentHasChangedException(
            (int)ExceptionIds.DocumentHasChanged,
            $"ETag '{eTag}' doesn't match the current value for document '{itemId}' on partitionKey '{partitionKey}' of collection '{containerName}' on database '{databaseName}'",
            databaseAccountUri, databaseName, containerName, partitionKey, itemId, eTag,
            exception
        );
    }
}

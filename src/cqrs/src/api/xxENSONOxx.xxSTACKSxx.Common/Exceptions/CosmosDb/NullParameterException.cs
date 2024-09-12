#if CosmosDb
using System;

namespace xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;

public class NullParameterException : CosmosDBException
{
    private NullParameterException(
        int exceptionCode,
        string message,
        string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
        Exception innerException
    ) : base(exceptionCode, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
    {
    }

    public static void Raise(string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, string parameter, Exception exception = null)
    {
        throw new NullParameterException(
            (int)ExceptionIds.NullParameter,
            $"The parameter {parameter} is required but was not provided. Database: {databaseName}. Container: {containerName}.",
            databaseAccountUri, databaseName, containerName, partitionKey, itemId,
            exception
        );
    }
}
#endif

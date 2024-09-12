#if CosmosDb
using System;

namespace xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;

public class InvalidSearchParameterException : CosmosDBException
{
    private InvalidSearchParameterException(
        int exceptionCode,
        string message,
        string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
        Exception innerException
    ) : base(exceptionCode, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
    {
    }

    public static void Raise(string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, string parameterName, string parameterValue, string expectedValue, Exception exception = null)
    {
        throw new InvalidSearchParameterException(
            (int)ExceptionIds.InvalidSearchParameter,
            expectedValue == null ?
                $"The search parameter {parameterName} is invalid. The value provided was {parameterValue ?? "(null)"}." :
                $"The search parameter {parameterName} is invalid. The value provided was {parameterValue ?? "(null)"}. Expected: {expectedValue}",
            databaseAccountUri, databaseName, containerName, partitionKey, itemId,
            exception
        );
    }
}
#endif

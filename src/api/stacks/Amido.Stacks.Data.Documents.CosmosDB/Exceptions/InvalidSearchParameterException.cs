using System;

namespace Amido.Stacks.Data.Documents.CosmosDB.Exceptions
{
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
                    $"The parameter {parameterName} is invalid. The value provided was {parameterValue ?? "(null)"}." :
                    $"The parameter {parameterName} is invalid. The value provided was {parameterValue ?? "(null)"}. Expected: {expectedValue}",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId,
                exception
            );
        }
    }
}

using System;

namespace Amido.Stacks.Data.Documents.CosmosDB.Exceptions
{
    public class PartitionKeyRequiredException : CosmosDBException
    {
        private PartitionKeyRequiredException(
            int exceptionCode,
            string message,
            string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
            Exception innerException
        ) : base(exceptionCode, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
        {
        }

        public static void Raise(string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, Exception exception = null)
        {
            throw new PartitionKeyRequiredException(
                (int)ExceptionIds.PartitionKeyRequired,
                $"The partition key is required but was not provided. Database: {databaseName}. Container: {containerName}. Item: {itemId}",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId,
                exception
            );
        }
    }
}

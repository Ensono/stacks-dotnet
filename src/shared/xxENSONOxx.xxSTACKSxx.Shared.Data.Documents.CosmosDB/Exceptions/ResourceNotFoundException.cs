using System;

namespace xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Exceptions
{
    public class ResourceNotFoundException : CosmosDBException
    {
        private ResourceNotFoundException(
            int exceptionCode,
            string message,
            string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
            Exception innerException
        ) : base(exceptionCode, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
        {
        }

        public string ETag { get; }

        public static void Raise(string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, Exception exception = null)
        {
            throw new ResourceNotFoundException(
                (int)ExceptionIds.ResourceNotFound,
                $"The document or resource requested couldn't be found. Resource id '{itemId}' on partitionKey '{partitionKey}' of collection '{containerName}' on database '{databaseName}'",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId,
                exception
            );
        }
    }
}

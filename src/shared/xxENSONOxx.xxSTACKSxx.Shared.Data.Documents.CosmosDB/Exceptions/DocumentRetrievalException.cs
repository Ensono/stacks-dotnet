using System;

namespace xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Exceptions
{
    public class DocumentRetrievalException : CosmosDBException
    {
        private DocumentRetrievalException(
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
            throw new DocumentRetrievalException(
                (int)ExceptionIds.DocumentRetrievalFailed,
                $"Failed to load the document '{itemId}' on partitionKey '{partitionKey}' of collection '{containerName}' on database '{databaseName}'",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId,
                exception
            );
        }
    }
}

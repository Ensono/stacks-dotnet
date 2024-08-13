using System;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Exceptions
{
    public class DocumentUpsertException : CosmosDBException
    {
        private DocumentUpsertException(
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
            throw new DocumentUpsertException(
                (int)ExceptionIds.DocumentUpsertFailed,
                $"Failed to upsert the document '{itemId}' on partitionKey '{partitionKey}' of collection '{containerName}' on database '{databaseName}'. {exception?.Message}",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId, eTag,
                exception
            );
        }
    }
}

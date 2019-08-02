﻿using System;

namespace Amido.Stacks.Data.Documents.CosmosDB.Exceptions
{
    public class DocumentDeletionException : CosmosDBException
    {
        private DocumentDeletionException(
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
            throw new DocumentDeletionException(
                (int)ExceptionIds.DocumentDeletionFailed,
                $"Failed to delete the document '{itemId}' on partitionKey {partitionKey}' of collection '{containerName}'",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId,
                exception
            );
        }
    }
}

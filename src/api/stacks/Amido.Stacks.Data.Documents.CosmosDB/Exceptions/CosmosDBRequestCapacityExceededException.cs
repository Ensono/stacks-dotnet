﻿using System;

namespace Amido.Stacks.Data.Documents.CosmosDB.Exceptions
{
    public class CosmosDBRequestCapacityExceededException : CosmosDBException
    {
        private CosmosDBRequestCapacityExceededException(
            int exceptionCode,
            string message,
            string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
            Exception innerException
        ) : base(exceptionCode, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
        {
        }

        public string ETag { get; }

        public override int HttpStatusCode => 503;

        public static void Raise(string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, Exception exception = null)
        {
            throw new CosmosDBRequestCapacityExceededException(
                (int)ExceptionIds.CosmosDBRequestCapacityExceeded,
                $"The database refused the request to the collection '{containerName}' because it exceeded the capacity allocated.",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId,
                exception
            );
        }
    }
}

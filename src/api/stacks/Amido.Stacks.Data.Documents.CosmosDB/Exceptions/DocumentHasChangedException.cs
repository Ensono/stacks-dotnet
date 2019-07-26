using System;
using Amido.Stacks.Core.Operations;

namespace Amido.Stacks.Data.Documents.CosmosDB.Exceptions
{
    public class DocumentHasChangedException : CosmosDBException
    {
        public DocumentHasChangedException(
            int exceptionCode, int operationCode, Guid correlationId, string message,
            string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, string eTag,
            Exception innerException
        ) : base(exceptionCode, operationCode, correlationId, message, databaseAccountUri, databaseName, containerName, partitionKey, itemId, innerException)
        {
            ETag = eTag;


            Data["ETag"] = ETag;
        }

        public string ETag { get; }

        public static void Raise(IOperationContext context, string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId, string eTag, Exception exception = null)
        {
            throw new DocumentHasChangedException(
                (int)ExceptionIds.DocumentHasChanged,
                context.OperationCode,
                context.CorrelationId,
                $"ETag '{eTag}' doesn't match the current value for document '{itemId}' on partitionKey {partitionKey}' of collection '{containerName}'",
                databaseAccountUri, databaseName, containerName, partitionKey, itemId, eTag,
                exception
                );
        }
    }
}

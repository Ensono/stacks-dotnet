using System;
using Amido.Stacks.Core.Exceptions;

namespace Amido.Stacks.Data.Documents.CosmosDB
{
    public abstract class CosmosDBException : InfrastructureExceptionBase
    {
        public CosmosDBException(
            int exceptionCode, 
            string message,
            string databaseAccountUri, string databaseName, string containerName, string partitionKey, string itemId,
            Exception innerException
        ) : base(exceptionCode, message, innerException)
        {
            DatabaseAccountUri = databaseAccountUri;
            DatabaseName = databaseName;
            ContainerName = containerName;
            PartitionKey = partitionKey;
            ItemId = itemId;


            Data["DatabaseAccountUri"] = DatabaseAccountUri;
            Data["DatabaseName"] = DatabaseName;
            Data["ContainerName"] = ContainerName;
            Data["PartitionKey"] = PartitionKey;
            Data["ItemId"] = ItemId;
        }

        public string DatabaseAccountUri { get; }
        public string DatabaseName { get; }
        public string ContainerName { get; }
        public string PartitionKey { get; }
        public string ItemId { get; }
    }
}

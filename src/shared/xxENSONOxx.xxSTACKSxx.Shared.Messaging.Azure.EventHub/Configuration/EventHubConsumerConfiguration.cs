using xxENSONOxx.xxSTACKSxx.Shared.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Configuration
{
    public class EventHubConsumerConfiguration : EventHubEntityConfiguration
    {
        /// <summary>
        /// Connection string for the blob storage account.
        /// </summary>
        public Secret BlobStorageConnectionString { get; set; }

        /// <summary>
        /// Blob storage container name.
        /// </summary>
        public string BlobContainerName { get; set; }
    }
}

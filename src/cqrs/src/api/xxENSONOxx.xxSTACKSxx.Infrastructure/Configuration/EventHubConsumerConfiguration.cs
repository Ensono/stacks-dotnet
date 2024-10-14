using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;

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

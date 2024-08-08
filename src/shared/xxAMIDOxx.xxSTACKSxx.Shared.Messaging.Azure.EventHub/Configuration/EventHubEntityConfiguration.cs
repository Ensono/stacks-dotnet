using Amido.Stacks.Configuration;

namespace Amido.Stacks.Messaging.Azure.EventHub.Configuration
{
    public abstract class EventHubEntityConfiguration
    {
        /// <summary>
        /// Connection string for the Event Hub namespace.
        /// </summary>
        public Secret NamespaceConnectionString { get; set; }

        /// <summary>
        /// Name of the Event Hub.
        /// </summary>
        public string EventHubName { get; set; }
    }
}

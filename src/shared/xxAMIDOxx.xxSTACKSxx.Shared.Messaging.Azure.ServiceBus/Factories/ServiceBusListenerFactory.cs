using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{

    public class ServiceBusListenerFactory : IServiceBusListenerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceBusListenerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IMessageListener Create<T>(ServiceBusQueueListenerConfiguration configuration)
        {
            return new ServiceBusListener(
                            serviceProvider.GetRequiredService<ILogger<ServiceBusListener>>(),
                            serviceProvider.GetRequiredService<IMessageReceiverClientFactory>(),
                            serviceProvider.GetRequiredService<IMessageProcessorFactory>(),
                            configuration);
        }
    }
}

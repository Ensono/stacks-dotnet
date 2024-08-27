﻿using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageProcessorFactory
    {
        IMessageProcessor Create(ServiceBusQueueListenerConfiguration configuration);
    }
}

using System;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageHandlerFactory
    {
        object CreateHandlerFor(Type type);
    }
}
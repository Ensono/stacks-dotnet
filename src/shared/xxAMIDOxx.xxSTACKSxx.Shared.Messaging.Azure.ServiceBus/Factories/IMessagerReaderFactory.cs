using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessagerReaderFactory
    {
        IMessageReader CreateReader<T>(string name = null);
    }
}

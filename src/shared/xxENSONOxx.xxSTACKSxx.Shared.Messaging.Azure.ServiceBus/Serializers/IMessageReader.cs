using Microsoft.Azure.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageReader
    {
        T Read<T>(Message message);
    }
}

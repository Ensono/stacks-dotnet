using Azure.Messaging.EventHubs;

namespace xxENSONOxx.xxSTACKSxx.Listener.Serialization;

public interface IMessageReader
{
    T Read<T>(EventData eventData);
}

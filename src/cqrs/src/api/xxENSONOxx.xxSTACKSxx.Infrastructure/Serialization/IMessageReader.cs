#if (EventPublisherEventHub)
using Azure.Messaging.EventHubs;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Serialization;

public interface IMessageReader
{
    T Read<T>(EventData eventData);
}
#endif

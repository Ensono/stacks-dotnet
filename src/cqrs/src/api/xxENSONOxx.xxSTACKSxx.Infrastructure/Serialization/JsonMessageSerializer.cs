#if (EventPublisherEventHub)
using System.Text;
using Azure.Messaging.EventHubs;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Serialization;

public class JsonMessageSerializer : IMessageReader
{
    public T Read<T>(EventData eventData)
    {
        var jsonBody = Encoding.UTF8.GetString(eventData.Body.ToArray());
        return JsonConvert.DeserializeObject<T>(jsonBody);
    }
}
#endif

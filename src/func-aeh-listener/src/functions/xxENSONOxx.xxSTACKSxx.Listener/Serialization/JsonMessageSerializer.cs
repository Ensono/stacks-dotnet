using System.Text;
using Azure.Messaging.EventHubs;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.Listener.Serialization;

public class JsonMessageSerializer : IMessageReader
{
    public T Read<T>(EventData eventData)
    {
        var jsonBody = Encoding.UTF8.GetString(eventData.Body.ToArray());
        return JsonConvert.DeserializeObject<T>(jsonBody);
    }
}

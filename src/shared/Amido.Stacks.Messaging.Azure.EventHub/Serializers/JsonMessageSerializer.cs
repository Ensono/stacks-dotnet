using Azure.Messaging.EventHubs;
using Newtonsoft.Json;
using System.Text;

namespace Amido.Stacks.Messaging.Azure.EventHub.Serializers
{
    public class JsonMessageSerializer : IMessageReader
    {
        public T Read<T>(EventData eventData)
        {
            var jsonBody = Encoding.UTF8.GetString(eventData.Body.ToArray());
            return JsonConvert.DeserializeObject<T>(jsonBody);
        }
    }
}
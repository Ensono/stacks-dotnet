using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Consumer
{
    public interface IEventConsumer
    {
        Task ProcessAsync();
    }
}
using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Consumer
{
    public interface IEventConsumer
    {
        Task ProcessAsync();
    }
}

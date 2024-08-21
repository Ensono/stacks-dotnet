using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners
{
    public interface IMessageListener
    {
        Task StartAsync();
        Task StopAsync();
    }
}

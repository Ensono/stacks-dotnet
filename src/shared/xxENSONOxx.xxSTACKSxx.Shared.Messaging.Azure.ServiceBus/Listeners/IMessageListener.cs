using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners
{
    public interface IMessageListener
    {
        Task StartAsync();
        Task StopAsync();
    }
}

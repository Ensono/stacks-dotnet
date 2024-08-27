using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IServiceBusSenderFactory
    {
        Task<IMessageSender> CreateAsync(ServiceBusSenderEntityConfiguration configuration);
    }
}

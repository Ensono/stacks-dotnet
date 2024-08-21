using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IServiceBusSenderFactory
    {
        Task<IMessageSender> CreateAsync(ServiceBusSenderEntityConfiguration configuration);
    }
}
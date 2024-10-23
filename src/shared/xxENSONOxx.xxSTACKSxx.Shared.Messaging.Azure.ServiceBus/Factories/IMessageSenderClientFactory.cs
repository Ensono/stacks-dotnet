using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Core;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageSenderClientFactory
    {
        Task<ISenderClient> CreateSenderClient(ServiceBusEntityConfiguration configuration);
    }
}

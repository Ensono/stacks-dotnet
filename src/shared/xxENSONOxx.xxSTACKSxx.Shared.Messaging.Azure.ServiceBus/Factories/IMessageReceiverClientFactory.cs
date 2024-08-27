using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using Microsoft.Azure.ServiceBus.Core;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageReceiverClientFactory
    {
        Task<IReceiverClient> CreateReceiverClient(ServiceBusEntityConfiguration configuration);
    }
}

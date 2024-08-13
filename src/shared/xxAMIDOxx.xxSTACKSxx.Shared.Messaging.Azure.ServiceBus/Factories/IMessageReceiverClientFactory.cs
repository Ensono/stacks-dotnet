using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using Microsoft.Azure.ServiceBus.Core;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageReceiverClientFactory
    {
        Task<IReceiverClient> CreateReceiverClient(ServiceBusEntityConfiguration configuration);
    }
}

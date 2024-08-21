using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using Microsoft.Azure.ServiceBus.Core;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageSenderClientFactory
    {
        Task<ISenderClient> CreateSenderClient(ServiceBusEntityConfiguration configuration);
    }
}
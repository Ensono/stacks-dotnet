using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

public interface IApplicationEventPublisher
{
    Task PublishAsync(IApplicationEvent applicationEvent);
}

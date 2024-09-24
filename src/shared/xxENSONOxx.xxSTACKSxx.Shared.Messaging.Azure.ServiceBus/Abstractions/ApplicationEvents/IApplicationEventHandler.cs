using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

public interface IApplicationEventHandler<in TApplicationEvent> where TApplicationEvent : IApplicationEvent
{
    Task HandleAsync(TApplicationEvent applicationEvent);
}

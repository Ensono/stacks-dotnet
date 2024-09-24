using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

public interface IApplicationEventHandler<in TApplicationEvent> where TApplicationEvent : IApplicationEvent
{
    Task HandleAsync(TApplicationEvent applicationEvent);
}

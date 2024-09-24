using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

public interface IApplicationEventPublisher
{
    Task PublishAsync(IApplicationEvent applicationEvent);
}

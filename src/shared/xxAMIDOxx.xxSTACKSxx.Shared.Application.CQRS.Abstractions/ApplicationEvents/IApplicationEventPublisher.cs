using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEventPublisher
    {
        Task PublishAsync(IApplicationEvent applicationEvent);
    }
}

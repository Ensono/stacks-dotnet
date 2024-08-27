using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEventPublisher
    {
        Task PublishAsync(IApplicationEvent applicationEvent);
    }
}

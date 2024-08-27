using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEventHandler<in TApplicationEvent> where TApplicationEvent : IApplicationEvent
    {
        Task HandleAsync(TApplicationEvent applicationEvent);
    }
}

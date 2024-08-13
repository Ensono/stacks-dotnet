using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEventHandler<in TApplicationEvent> where TApplicationEvent : IApplicationEvent
    {
        Task HandleAsync(TApplicationEvent applicationEvent);
    }
}

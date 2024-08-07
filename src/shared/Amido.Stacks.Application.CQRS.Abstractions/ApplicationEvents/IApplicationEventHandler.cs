using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEventHandler<in TApplicationEvent> where TApplicationEvent : IApplicationEvent
    {
        Task HandleAsync(TApplicationEvent applicationEvent);
    }
}

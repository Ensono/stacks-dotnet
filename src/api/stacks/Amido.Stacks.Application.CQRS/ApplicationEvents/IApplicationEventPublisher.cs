using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEventPublisher
    {
        Task PublishAsync(IApplicationEvent applicationEvent);
    }
}

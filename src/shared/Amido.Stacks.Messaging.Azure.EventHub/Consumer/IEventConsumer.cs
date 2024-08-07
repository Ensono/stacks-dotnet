using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.EventHub.Consumer
{
    public interface IEventConsumer
    {
        Task ProcessAsync();
    }
}
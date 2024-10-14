using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task SendAsync(ICommand command);
}

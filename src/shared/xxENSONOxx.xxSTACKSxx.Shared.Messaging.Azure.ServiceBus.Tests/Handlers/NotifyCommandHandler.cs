using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Handlers;

public class NotifyCommandHandler(TestDependency.ITestable<NotifyCommand> testable) : ICommandHandler<NotifyCommand, bool>
{
    public Task<bool> HandleAsync(NotifyCommand command)
    {
        testable.Complete(command);
        return Task.FromResult(true);
    }
}

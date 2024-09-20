using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers
{
    public class NotifyCommandHandler(ITestable<NotifyCommand> testable) : ICommandHandler<NotifyCommand, bool>
    {
        public Task<bool> HandleAsync(NotifyCommand command)
        {
            testable.Complete(command);
            return Task.FromResult(true);
        }
    }
}

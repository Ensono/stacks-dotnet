using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers
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

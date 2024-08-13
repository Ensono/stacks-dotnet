using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers
{
    public class NotifyCommandHandler : ICommandHandler<NotifyCommand, bool>
    {
        private readonly ITestable<NotifyCommand> _testable;

        public NotifyCommandHandler(ITestable<NotifyCommand> testable)
        {
            _testable = testable;
        }

        public Task<bool> HandleAsync(NotifyCommand command)
        {
            _testable.Complete(command);
            return Task.FromResult(true);
        }
    }
}

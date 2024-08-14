using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Events;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers
{
    public class NotifyEventHandler : IApplicationEventHandler<NotifyEvent>
    {
        private readonly ITestable<NotifyEvent> _testable;

        public NotifyEventHandler(ITestable<NotifyEvent> testable)
        {
            _testable = testable;
        }

        public Task HandleAsync(NotifyEvent applicationEvent)
        {
            _testable.Complete(applicationEvent);
            return Task.CompletedTask;
        }
    }
}
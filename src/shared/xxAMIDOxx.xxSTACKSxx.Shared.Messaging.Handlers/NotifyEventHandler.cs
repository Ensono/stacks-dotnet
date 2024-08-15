using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Events;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers
{
    public class NotifyEventHandler(ITestable<NotifyEvent> testable) : IApplicationEventHandler<NotifyEvent>
    {
        public Task HandleAsync(NotifyEvent applicationEvent)
        {
            testable.Complete(applicationEvent);
            return Task.CompletedTask;
        }
    }
}

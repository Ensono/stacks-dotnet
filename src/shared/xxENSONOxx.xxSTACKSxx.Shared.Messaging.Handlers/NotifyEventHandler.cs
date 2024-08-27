using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers
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

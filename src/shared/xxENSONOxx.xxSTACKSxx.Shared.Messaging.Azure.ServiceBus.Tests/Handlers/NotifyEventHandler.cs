using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Handlers;

public class NotifyEventHandler(TestDependency.ITestable<NotifyEvent> testable) 
           : IApplicationEventHandler<NotifyEvent>
{
    public Task HandleAsync(NotifyEvent applicationEvent)
    {
        testable.Complete(applicationEvent);
        return Task.CompletedTask;
    }
}

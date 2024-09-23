using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Handlers
{
    /// <summary>
    /// Represents a custom message processor used
    /// </summary>
    public class CustomMessageProcessor(Azure.ServiceBus.Tests.Handlers.TestDependency.ITestable<NotifyEvent> testable) : IMessageProcessor
    {
        public Task ProcessAsync(Message message, CancellationToken cancellationToken)
        {
            //Do what you gotta do, you are on your own now

            NotifyEvent eventMsg = new NotifyEvent(Guid.Parse(message.CorrelationId), 098);

            testable.Complete(eventMsg);

            return Task.CompletedTask;
        }
    }
}

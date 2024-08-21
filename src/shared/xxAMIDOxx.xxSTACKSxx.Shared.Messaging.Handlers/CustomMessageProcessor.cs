using System;
using System.Threading;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Events;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;
using Microsoft.Azure.ServiceBus;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Handlers
{
    /// <summary>
    /// Represents a custom message processor used
    /// </summary>
    public class CustomMessageProcessor(ITestable<NotifyEvent> testable) : IMessageProcessor
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

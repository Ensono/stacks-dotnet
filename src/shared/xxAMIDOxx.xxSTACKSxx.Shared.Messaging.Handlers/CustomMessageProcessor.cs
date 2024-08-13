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
    public class CustomMessageProcessor : IMessageProcessor
    {
        private readonly ITestable<NotifyEvent> testable;

        public CustomMessageProcessor(ITestable<NotifyEvent> testable)
        {
            this.testable = testable;
        }

        public Task ProcessAsync(Message message, CancellationToken cancellationToken)
        {
            //Do what you gotta do, you are on your own now

            NotifyEvent eventMsg = new NotifyEvent(Guid.Parse(message.CorrelationId), 098);

            this.testable.Complete(eventMsg);

            return Task.CompletedTask;
        }
    }
}

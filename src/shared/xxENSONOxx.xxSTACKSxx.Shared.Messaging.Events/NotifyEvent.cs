using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Events
{
    public class NotifyEvent(Guid correlationId, int operationCode, string sessionId = null, string subject = null)
        : IApplicationEvent, ICloudEvent, ISessionContext
    {
        public int EventCode => 123;
        public Guid CorrelationId { get; } = correlationId;
        public int OperationCode { get; } = operationCode;
        public string Id { get; } = Guid.NewGuid().ToString();
        public DateTime? Time { get; } = DateTime.UtcNow;
        public string Subject { get; set; } = subject;
        public string SessionId { get; set; } = sessionId;
    }
}

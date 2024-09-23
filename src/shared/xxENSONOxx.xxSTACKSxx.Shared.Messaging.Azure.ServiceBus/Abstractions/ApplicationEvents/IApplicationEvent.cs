using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

public interface IApplicationEvent : IOperationContext
{
    /// <summary>
    /// Unique code of event raised
    /// </summary>
    int EventCode { get; }
}

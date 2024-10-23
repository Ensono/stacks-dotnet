using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

public interface IApplicationEvent : IOperationContext
{
    /// <summary>
    /// Unique code of event raised
    /// </summary>
    int EventCode { get; }
}

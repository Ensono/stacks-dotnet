using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;

public interface IApplicationEvent : IOperationContext
{
    /// <summary>
    /// Unique code of event raised
    /// </summary>
    int EventCode { get; }
}

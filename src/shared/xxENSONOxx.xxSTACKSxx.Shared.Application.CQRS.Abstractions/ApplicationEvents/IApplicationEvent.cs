using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEvent : IOperationContext
    {
        /// <summary>
        /// Unique code of event raised
        /// </summary>
        int EventCode { get; }
    }
}

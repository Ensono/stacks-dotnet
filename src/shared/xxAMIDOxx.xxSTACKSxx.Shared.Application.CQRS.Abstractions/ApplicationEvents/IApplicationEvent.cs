using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEvent : IOperationContext
    {
        /// <summary>
        /// Unique code of event raised
        /// </summary>
        int EventCode { get; }
    }
}

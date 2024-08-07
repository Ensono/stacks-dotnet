using Amido.Stacks.Core.Operations;

namespace Amido.Stacks.Application.CQRS.ApplicationEvents
{
    public interface IApplicationEvent : IOperationContext
    {
        /// <summary>
        /// Unique code of event raised
        /// </summary>
        int EventCode { get; }
    }
}

using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

[method: JsonConstructor]
public class MenuUpdatedEvent(int operationCode, Guid correlationId, Guid menuId)
           : IApplicationEvent
{
    public MenuUpdatedEvent(IOperationContext context, Guid menuId)
         : this(context.OperationCode, context.CorrelationId, menuId)
    {
    }

    public int EventCode => (int)ApplicationEvents.EventCode.MenuUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;
}

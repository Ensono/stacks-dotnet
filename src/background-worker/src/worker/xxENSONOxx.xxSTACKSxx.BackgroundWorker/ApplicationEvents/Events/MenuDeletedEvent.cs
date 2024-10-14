using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

[method: JsonConstructor]
public class MenuDeletedEvent(int operationCode, Guid correlationId, Guid menuId)
           : IApplicationEvent
{
    public MenuDeletedEvent(IOperationContext context, Guid menuId)
         : this(context.OperationCode, context.CorrelationId, menuId)
    {
    }

    public int EventCode => (int)ApplicationEvents.EventCode.MenuDeleted;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;
}

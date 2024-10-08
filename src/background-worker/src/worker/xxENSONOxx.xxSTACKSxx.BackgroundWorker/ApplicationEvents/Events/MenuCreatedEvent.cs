using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

[method: JsonConstructor]
public class MenuCreatedEvent(int operationCode, Guid correlationId, Guid menuId)
           : IApplicationEvent
{
    public MenuCreatedEvent(IOperationContext context, Guid menuId)
         : this(context.OperationCode, context.CorrelationId, menuId)
    {
    }

    public int EventCode => (int)ApplicationEvents.EventCode.MenuCreated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;
}
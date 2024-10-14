using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

[method: JsonConstructor]
public class MenuItemUpdatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
           : IApplicationEvent
{
    public MenuItemUpdatedEvent(IOperationContext context, Guid menuId, Guid categoryId, Guid menuItemId)
         : this(context.OperationCode, context.CorrelationId, menuId, categoryId, menuItemId)
    {
    }

    public int EventCode => (int)ApplicationEvents.EventCode.MenuItemUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;

    public Guid CategoryId { get; } = categoryId;

    public Guid MenuItemId { get; } = menuItemId;
}

using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Operations;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

[method: JsonConstructor]
public class MenuItemDeletedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
    : IApplicationEvent
{
    public MenuItemDeletedEvent(IOperationContext context, Guid menuId, Guid categoryId, Guid menuItemId) : this(context.OperationCode, context.CorrelationId, menuId, categoryId, menuItemId)
    {
    }

    public int EventCode => (int)Enums.EventCode.MenuItemDeleted;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;

    public Guid MenuItemId { get; set; } = menuItemId;
}

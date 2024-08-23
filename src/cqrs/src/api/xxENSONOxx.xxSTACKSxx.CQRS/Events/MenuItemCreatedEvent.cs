using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;

[method: JsonConstructor]
public class MenuItemCreatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
    : IApplicationEvent
{
    public MenuItemCreatedEvent(IOperationContext context, Guid menuId, Guid categoryId, Guid menuItemId) : this(context.OperationCode, context.CorrelationId, menuId, categoryId, menuItemId)
    {
    }

    public int EventCode => (int)Enums.EventCode.MenuItemCreated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;

    public Guid CategoryId { get; } = categoryId;

    public Guid MenuItemId { get; } = menuItemId;
}

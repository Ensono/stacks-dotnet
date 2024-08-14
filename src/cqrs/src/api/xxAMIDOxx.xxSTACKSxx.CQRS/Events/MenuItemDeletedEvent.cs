using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;

public class MenuItemDeletedEvent : IApplicationEvent
{
    [JsonConstructor]
    public MenuItemDeletedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
    {
        OperationCode = operationCode;
        CorrelationId = correlationId;
        MenuId = menuId;
        CategoryId = categoryId;
        MenuItemId = menuItemId;
    }

    public MenuItemDeletedEvent(IOperationContext context, Guid menuId, Guid categoryId, Guid menuItemId)
    {
        OperationCode = context.OperationCode;
        CorrelationId = context.CorrelationId;
        MenuId = menuId;
        CategoryId = categoryId;
        MenuItemId = menuItemId;
    }

    public int EventCode => (int)Enums.EventCode.MenuItemDeleted;

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public Guid MenuId { get; }

    public Guid CategoryId { get; }

    public Guid MenuItemId { get; }
}

using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

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

    public Guid MenuId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid MenuItemId { get; set; }
}

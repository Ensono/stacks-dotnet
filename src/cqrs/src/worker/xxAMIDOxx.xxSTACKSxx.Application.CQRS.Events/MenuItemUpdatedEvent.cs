using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

public class MenuItemUpdatedEvent : IApplicationEvent
{
    [JsonConstructor]
    public MenuItemUpdatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
    {
        OperationCode = operationCode;
        CorrelationId = correlationId;
        MenuId = menuId;
        CategoryId = categoryId;
        MenuItemId = menuItemId;
    }

    public MenuItemUpdatedEvent(IOperationContext context, Guid menuId, Guid categoryId, Guid menuItemId)
    {
        OperationCode = context.OperationCode;
        CorrelationId = context.CorrelationId;
        MenuId = menuId;
        CategoryId = categoryId;
        MenuItemId = menuItemId;
    }

    public int EventCode => (int)Enums.EventCode.MenuItemUpdated;

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public Guid MenuId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid MenuItemId { get; set; }
}

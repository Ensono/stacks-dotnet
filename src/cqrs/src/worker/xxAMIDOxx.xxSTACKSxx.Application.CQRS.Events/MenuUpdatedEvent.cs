using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

public class MenuUpdatedEvent : IApplicationEvent
{
    [JsonConstructor]
    public MenuUpdatedEvent(int operationCode, Guid correlationId, Guid menuId)
    {
        OperationCode = operationCode;
        CorrelationId = correlationId;
        MenuId = menuId;
    }

    public MenuUpdatedEvent(IOperationContext context, Guid menuId)
    {
        OperationCode = context.OperationCode;
        CorrelationId = context.CorrelationId;
        MenuId = menuId;
    }

    public int EventCode => (int)Enums.EventCode.MenuUpdated;

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public Guid MenuId { get; set; }
}

using System;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

[method: JsonConstructor]
public class MenuUpdatedEvent(int operationCode, Guid correlationId, Guid menuId) : IApplicationEvent
{
    public MenuUpdatedEvent(IOperationContext context, Guid menuId) : this(context.OperationCode, context.CorrelationId, menuId)
    {
    }

    public int EventCode => (int)Enums.EventCode.MenuUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;
}

using System;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;

[method: JsonConstructor]
public class MenuCreatedEvent(int operationCode, Guid correlationId, Guid menuId) : IApplicationEvent
{
    public MenuCreatedEvent(IOperationContext context, Guid menuId) : this(context.OperationCode, context.CorrelationId, menuId)
    {
    }

    public int EventCode => (int)Enums.EventCode.MenuCreated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;
}

using System;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

[method: JsonConstructor]
public class CategoryUpdatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId)
    : IApplicationEvent
{
    public CategoryUpdatedEvent(IOperationContext context, Guid menuId, Guid categoryId) : this(context.OperationCode, context.CorrelationId, menuId, categoryId)
    {
    }

    public int EventCode => (int)Enums.EventCode.CategoryUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;
}

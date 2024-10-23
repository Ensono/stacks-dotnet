using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;

[method: JsonConstructor]
public class CategoryCreatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId)
    : IApplicationEvent
{
    public CategoryCreatedEvent(IOperationContext context, Guid menuId, Guid categoryId) : this(context.OperationCode, context.CorrelationId, menuId, categoryId)
    {
    }

    public int EventCode => (int)Enums.EventCode.CategoryCreated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;

    public Guid CategoryId { get; } = categoryId;
}

using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

[method: JsonConstructor]
public class CategoryDeletedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId)
    : IApplicationEvent
{
    public CategoryDeletedEvent(IOperationContext context, Guid menuId, Guid categoryId) : this(context.OperationCode, context.CorrelationId, menuId, categoryId)
    {
    }

    public int EventCode => (int)Enums.EventCode.CategoryDeleted;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;
}

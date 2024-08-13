using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;

public class CategoryUpdatedEvent : IApplicationEvent
{
    [JsonConstructor]
    public CategoryUpdatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId)
    {
        OperationCode = operationCode;
        CorrelationId = correlationId;
        MenuId = menuId;
        CategoryId = categoryId;
    }

    public CategoryUpdatedEvent(IOperationContext context, Guid menuId, Guid categoryId)
    {
        OperationCode = context.OperationCode;
        CorrelationId = context.CorrelationId;
        MenuId = menuId;
        CategoryId = categoryId;
    }

    public int EventCode => (int)Enums.EventCode.CategoryUpdated;

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public Guid MenuId { get; }

    public Guid CategoryId { get; }
}

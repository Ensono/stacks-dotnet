using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Operations;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

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

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;
}

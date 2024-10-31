using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

[method: JsonConstructor]
public class CategoryUpdatedEvent(int operationCode, Guid correlationId, Guid menuId, Guid categoryId)
           : IApplicationEvent
{
    public CategoryUpdatedEvent(IOperationContext context, Guid menuId, Guid categoryId)
         : this(context.OperationCode, context.CorrelationId, menuId, categoryId)
    {
    }

    public int EventCode => (int)ApplicationEvents.EventCode.CategoryUpdated;

    public int OperationCode { get; } = operationCode;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; } = menuId;

    public Guid CategoryId { get; } = categoryId;
}

using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class DeleteCategory(Guid correlationId, Guid menuId, Guid categoryId) : ICategoryCommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.DeleteCategory;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;
}

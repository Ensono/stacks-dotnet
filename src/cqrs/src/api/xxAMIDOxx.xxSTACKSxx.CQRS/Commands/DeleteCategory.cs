using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

public class DeleteCategory : ICategoryCommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.DeleteCategory;

    public Guid CorrelationId { get; }

    public Guid MenuId { get; set; }

    public Guid CategoryId { get; set; }

    public DeleteCategory(Guid correlationId, Guid menuId, Guid categoryId)
    {
        CorrelationId = correlationId;
        MenuId = menuId;
        CategoryId = categoryId;
    }
}

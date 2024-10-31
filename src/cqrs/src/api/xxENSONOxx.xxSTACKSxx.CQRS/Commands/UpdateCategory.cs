using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class UpdateCategory(Guid correlationId, Guid menuId, Guid categoryId, string name, string description)
    : ICategoryCommand
{
    public int OperationCode => (int)Shared.Abstractions.Operations.OperationCode.UpdateCategory;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;
}

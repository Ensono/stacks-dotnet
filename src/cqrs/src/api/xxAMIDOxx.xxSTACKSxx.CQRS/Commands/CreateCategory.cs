using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

public class CreateCategory(Guid correlationId, Guid menuId, string name, string description)
    : IMenuCommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.CreateCategory;

    public Guid CorrelationId { get; set; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;
}

using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

public class CreateCategory : IMenuCommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.CreateCategory;

    public Guid CorrelationId { get; set; }

    public Guid MenuId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public CreateCategory(Guid correlationId, Guid menuId, string name, string description)
    {
        CorrelationId = correlationId;
        MenuId = menuId;
        Name = name;
        Description = description;
    }
}

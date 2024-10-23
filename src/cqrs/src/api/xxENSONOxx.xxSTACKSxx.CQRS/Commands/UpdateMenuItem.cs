using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class UpdateMenuItem(
    Guid correlationId,
    Guid menuId,
    Guid categoryId,
    Guid menuItemId,
    string name,
    string description,
    double price,
    bool available)
    : IMenuItemCommand
{
    public int OperationCode => (int)Shared.Abstractions.Operations.OperationCode.UpdateMenuItem;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;

    public Guid MenuItemId { get; set; } = menuItemId;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public double Price { get; set; } = price;

    public bool Available { get; set; } = available;
}

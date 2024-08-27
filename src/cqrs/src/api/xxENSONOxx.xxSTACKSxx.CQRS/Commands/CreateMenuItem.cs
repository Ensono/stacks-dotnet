using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class CreateMenuItem(
    Guid correlationId,
    Guid menuId,
    Guid categoryId,
    string name,
    string description,
    double price,
    bool available)
    : ICategoryCommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.CreateMenuItem;

    public Guid CorrelationId { get; set; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public double Price { get; set; } = price;

    public bool Available { get; set; } = available;
}

using System;

namespace xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Entities;

public class MenuItem(Guid id, string name, string description, double price, bool available)
    : IEntity<Guid>
{
    public Guid Id { get; set; } = id;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public double Price { get; set; } = price;

    public bool Available { get; set; } = available;
}

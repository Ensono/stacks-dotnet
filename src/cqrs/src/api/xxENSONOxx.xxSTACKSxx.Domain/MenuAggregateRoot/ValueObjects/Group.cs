using System;

namespace xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.ValueObjects;

public class Group(Guid id, string name) : IValueObject
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}

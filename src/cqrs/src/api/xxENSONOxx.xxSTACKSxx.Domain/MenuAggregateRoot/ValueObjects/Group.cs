using System;
using xxENSONOxx.xxSTACKSxx.Shared.Domain;

namespace xxENSONOxx.xxSTACKSxx.Domain.ValueObjects;

public class Group(Guid id, string name) : IValueObject
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}

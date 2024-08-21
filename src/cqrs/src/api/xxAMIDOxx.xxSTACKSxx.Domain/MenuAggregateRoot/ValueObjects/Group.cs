using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Domain.ValueObjects;

public class Group(Guid id, string name) : IValueObject
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}

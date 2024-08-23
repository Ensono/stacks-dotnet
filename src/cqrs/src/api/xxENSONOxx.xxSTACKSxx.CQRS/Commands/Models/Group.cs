using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands.Models;

public class Group(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}

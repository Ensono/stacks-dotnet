using System;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.UnitTests;

public class FakeMenu(Guid id)
{
	public FakeMenu() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}

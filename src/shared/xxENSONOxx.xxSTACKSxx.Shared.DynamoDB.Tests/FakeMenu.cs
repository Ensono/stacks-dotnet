using System;

namespace xxENSONOxx.xxSTACKSxx.Shared.DynamoDB.Tests;

public class FakeMenu(Guid id)
{
	public FakeMenu() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}

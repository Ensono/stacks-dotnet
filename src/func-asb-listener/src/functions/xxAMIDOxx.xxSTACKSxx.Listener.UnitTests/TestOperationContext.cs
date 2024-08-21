using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Listener.UnitTests;

public class TestOperationContext : IOperationContext
{
    public TestOperationContext() {}

    public int OperationCode => 999;

    public Guid CorrelationId => Guid.NewGuid();
}

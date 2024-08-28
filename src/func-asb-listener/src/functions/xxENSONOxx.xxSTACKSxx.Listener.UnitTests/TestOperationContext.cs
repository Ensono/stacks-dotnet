﻿using System;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxENSONOxx.xxSTACKSxx.Listener.UnitTests;

public class TestOperationContext : IOperationContext
{
    public TestOperationContext() {}

    public int OperationCode => 999;

    public Guid CorrelationId => Guid.NewGuid();
}
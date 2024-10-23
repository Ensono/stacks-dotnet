using System;
using xxENSONOxx.xxSTACKSxx.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands;

public class NotifyClientCommandWithoutHandler : ICommand
{
    public int OperationCode { get; }
    public Guid CorrelationId { get; }
}

using System;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands;

public class NotifyCommand(Guid correlationId, string testMember) : ICommand
{
    public string TestMember { get; } = testMember;
    public int OperationCode { get; } = 666;
    public Guid CorrelationId { get; } = correlationId;
}

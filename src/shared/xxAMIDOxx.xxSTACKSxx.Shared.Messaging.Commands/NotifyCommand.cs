using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands
{
    public class NotifyCommand(Guid correlationId, string testMember) : ICommand
    {
        public string TestMember { get; } = testMember;
        public int OperationCode { get; } = 666;
        public Guid CorrelationId { get; } = correlationId;
    }
}

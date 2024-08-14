using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands
{
    public class NotifyCommand : ICommand
    {
        public NotifyCommand(Guid correlationId, string testMember)
        {
            OperationCode = 666;
            CorrelationId = correlationId;
            TestMember = testMember;
        }

        public string TestMember { get; }
        public int OperationCode { get; }
        public Guid CorrelationId { get; }
    }
}

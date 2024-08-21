using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands
{
    public class NotifyClientCommandWithoutHandler : ICommand
    {
        public int OperationCode { get; }
        public Guid CorrelationId { get; }
    }
}
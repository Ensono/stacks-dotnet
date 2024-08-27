using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Commands
{
    public class NotifyClientCommandWithoutHandler : ICommand
    {
        public int OperationCode { get; }
        public Guid CorrelationId { get; }
    }
}

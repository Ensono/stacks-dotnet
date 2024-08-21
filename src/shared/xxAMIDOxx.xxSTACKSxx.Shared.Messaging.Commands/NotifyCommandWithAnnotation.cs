using System;
using System.ComponentModel.DataAnnotations;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands
{
    public class NotifyCommandWithAnnotation(Guid correlationId, string testProperty) : ICommand
    {
        public int OperationCode => 536;

        [Required]
        public Guid CorrelationId { get; } = correlationId;

        [Required]
        public string TestProperty { get; } = testProperty;
    }
}

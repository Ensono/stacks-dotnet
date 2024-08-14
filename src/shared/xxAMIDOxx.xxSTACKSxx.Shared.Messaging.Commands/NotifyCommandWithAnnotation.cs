using System;
using System.ComponentModel.DataAnnotations;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Commands
{
    public class NotifyCommandWithAnnotation : ICommand
    {
        public NotifyCommandWithAnnotation(Guid correlationId, string testProperty)
        {
            CorrelationId = correlationId;
            TestProperty = testProperty;
        }

        public int OperationCode => 536;

        [Required]
        public Guid CorrelationId { get; }

        [Required]
        public string TestProperty { get; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands;

public class NotifyCommandWithAnnotation(Guid correlationId, string testProperty) : ICommand
{
    public int OperationCode => 536;

    [Required]
    public Guid CorrelationId { get; } = correlationId;

    [Required]
    public string TestProperty { get; } = testProperty;
}

using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Operations;

namespace xxENSONOxx.xxSTACKSxx.Listener.UnitTests;

[Trait("TestType", "UnitTests")]
public class StacksListenerTests
{
    private readonly ILogger<StacksListener> logger = Substitute.For<ILogger<StacksListener>>();

    [Fact]
    public void Run_ValidMessage_LogsMenuId()
    {
        // Arrange
        var messageId = Guid.NewGuid();
        var messageBody = new MenuCreatedEvent(new TestOperationContext(), messageId);
        var message = BuildMessage(messageBody);

        var stacksListener = new StacksListener(logger);

        // Act
        stacksListener.Run(message);

        // Assert
        logger.Received(1).LogInformation($"Message read. Menu Id: {message.MessageId}");
    }

    private static ServiceBusReceivedMessage BuildMessage(MenuCreatedEvent body)
    {
        var correlationId = GetCorrelationId(body);

        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
            new BinaryData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body))),
            contentType: "application/json;charset=utf-8",
            correlationId: $"{correlationId}");

        return message;
    }

    private static Guid GetCorrelationId(object body)
    {
        var operationContext = body as IOperationContext;

        return operationContext?.CorrelationId ?? Guid.NewGuid();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Listener.Serialization;

namespace xxENSONOxx.xxSTACKSxx.Listener.UnitTests;

[Trait("TestType", "UnitTests")]
public class StacksListenerTests
{
    private readonly IMessageReader msgReader;
    private readonly ILogger<StacksListener> logger;

    public StacksListenerTests()
    {
        msgReader = Substitute.For<IMessageReader>();
        logger = Substitute.For<ILogger<StacksListener>>();
    }

    [Fact]
    public void Run_ShouldNotCallRead_WhenEventDataIsEmpty()
    {
        var eventData = new EventData[0];
        var stacksListener = new StacksListener(msgReader, logger);

        stacksListener.Run(eventData);

        msgReader.DidNotReceive().Read<MenuCreatedEvent>(Arg.Any<EventData>());
    }

    [Fact]
    public void Run_ShouldProcessMultipleEvents()
    {
        var eventData = new List<EventData>();
        var msgBody1 = BuildMessageBody();
        var msgBody2 = BuildMessageBody();
        var message1 = BuildEventData(msgBody1);
        var message2 = BuildEventData(msgBody2);
        eventData.Add(message1);
        eventData.Add(message2);

        var stacksListener = new StacksListener(msgReader, logger);

        stacksListener.Run(eventData.ToArray());

        msgReader.Received(1).Read<MenuCreatedEvent>(message1);
        msgReader.Received(1).Read<MenuCreatedEvent>(message2);
    }
    
    [Fact]
    public void TestExecution()
    {
        var eventData = new List<EventData>();

        var msgBody = BuildMessageBody();
        var message = BuildEventData(msgBody);

        eventData.Add(message);

        var stacksListener = new StacksListener(msgReader, logger);

        stacksListener.Run(eventData.ToArray());

        msgReader.Received(1).Read<MenuCreatedEvent>(message);
    }

    public MenuCreatedEvent BuildMessageBody()
    {
        var id = Guid.NewGuid();
        return new MenuCreatedEvent(new TestOperationContext(), id);
    }

    public EventData BuildEventData(MenuCreatedEvent body)
    {
        var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

        var eventData = new EventData(byteArray);

        return eventData;
    }
}

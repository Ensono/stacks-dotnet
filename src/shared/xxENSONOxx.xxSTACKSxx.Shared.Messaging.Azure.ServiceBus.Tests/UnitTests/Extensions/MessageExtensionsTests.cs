using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Azure.ServiceBus;
using Shouldly;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Extensions;
using DummyEventSb = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events.DummyEventSb;
using NotifyCommand = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands.NotifyCommand;
using NotifyEvent = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events.NotifyEvent;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.UnitTests.Extensions;

public class MessageExtensionsTests
{
    [Theory]
    [ClassData(typeof(MessageExtensionTestsData))]
    public void MessageExtensionReturnsTheMessage(Message message, string expected, string testCase)
    {
        var result = message.GetEnclosedMessageType();
        result.ShouldBe(expected, false, testCase);
    }

    [Fact]
    public void MessageExtensionSetEnclosedMessageType()
    {
        var message = new Message();
        message.SetEnclosedMessageType(typeof(NotifyCommand))
            .SetEnclosedMessageType(typeof(NotifyCommand));
        message.UserProperties.Count.ShouldBe(1);
        message.UserProperties["EnclosedMessageType"].ShouldBe("xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands.NotifyCommand, xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests");
    }
        
    [Fact]
    public void MessageExtensionSetSessionId()
    {
        var message = new Message();
        message.SetSessionId(new NotifyEvent(Guid.NewGuid(), 1, "session-id"));
        message.SessionId.ShouldBe("session-id");
    }
        
    [Fact]
    public void MessageExtensionSetSessionIdIgnoresMissingSessionId()
    {
        var message = new Message();
        message.SetSessionId(new DummyEventSb());
        message.SessionId.ShouldBeNull();
    }
}

public class MessageExtensionTestsData : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]
        {
            new Message
            {
                UserProperties =
                {
                    new KeyValuePair<string, object>(nameof(MessageProperties.EnclosedMessageType),
                        "this.is.my.test.assembly, assembly")
                }
            },
            "this.is.my.test.assembly, assembly",
            "It is correctly set"
        },
        new object[]
        {
            new Message(),
            null,
            "It should be null as the enclosedMessageType is not set"
        },
        new object[]
        {
            new Message
            {
                UserProperties =
                {
                    new KeyValuePair<string, object>(nameof(MessageProperties.EnclosedMessageType),
                        42)
                }
            },
            null,
            "It should be null as the enclosedMessageType is not string"
        }
    };

    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

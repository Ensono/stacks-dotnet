using System;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;
using Shouldly;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.Commands;
using NotifyCommand = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands.NotifyCommand;
using NotifyEvent = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events.NotifyEvent;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.UnitTests.Deserializers;

public class CloudEventMessageReaderTests
{
    [Fact]
    public void GivenTheParametersCorrectTheBodyOfTheCommandWillBeParsed()
    {
        var serializer = new CloudEventMessageSerializer();

        var correlationId = Guid.NewGuid();
        var testMember = Guid.NewGuid().ToString();

        var message = serializer.Build<ICommand>(new NotifyCommand(correlationId, testMember));

        var result = serializer.Read<ICommand>(message) as NotifyCommand;

        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NotifyCommand));
        result.CorrelationId.ShouldBe(correlationId);
        result.TestMember.ShouldBe(testMember);
    }

    [Fact]
    public void GivenTheParametersCorrectTheBodyOfTheApplicationEventWillBeParsed()
    {
        var serializer = new CloudEventMessageSerializer();

        var correlationId = Guid.NewGuid();
        var message = serializer.Build(new NotifyEvent(correlationId, 321, "session-id"));

        var result = serializer.Read<IApplicationEvent>(message) as NotifyEvent;

        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NotifyEvent));
        result.EventCode.ShouldBe(123);
        result.CorrelationId.ShouldBe(correlationId);
        result.OperationCode.ShouldBe(321);
        result.SessionId.ShouldBe("session-id");
    }
}

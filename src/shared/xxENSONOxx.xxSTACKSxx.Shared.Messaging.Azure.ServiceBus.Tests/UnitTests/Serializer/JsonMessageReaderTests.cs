using System;
using System.Text;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Extensions;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using NotifyCommand = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands.NotifyCommand;
using NotifyEvent = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events.NotifyEvent;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.UnitTests.Deserializers
{
    public class JsonMessageReaderTests
    {
        [Fact]
        public void GivenTheAssemblyNameIsIncorrectAndCannotSerialiseItThrows()
        {
            var parser = new JsonMessageSerializer();
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NotifyCommand(new Guid("C8A14B73F2A14696BEEFBD432AF27296"), "669851d4-9836-4e1c-9787-bd914705c4dc")))
            };

            message.SetEnclosedMessageType(typeof(JsonMessageReaderTests));

            ShouldThrowExtensions.ShouldThrow<MessageParsingException>(() => parser.Read<NotifyCommand>(message) as NotifyCommand);
        }

        [Fact]
        public void GivenTheParametersCorrectTheBodyOfTheCommandWillBeParsed()
        {
            var parser = new JsonMessageSerializer();
            var correlationId = Guid.NewGuid();
            var testMember = Guid.NewGuid().ToString();
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NotifyCommand(correlationId, testMember)))
            };

            message.SetEnclosedMessageType(typeof(NotifyCommand));

            var result = parser.Read<NotifyCommand>(message) as NotifyCommand;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyCommand));
            result.CorrelationId.ShouldBe(correlationId);
            result.TestMember.ShouldBe(testMember);
        }

        [Fact]
        public void GivenTheParametersCorrectTheBodyOfTheApplicationEventWillBeParsed()
        {
            var parser = new JsonMessageSerializer();
            var correlationId = Guid.NewGuid();
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NotifyEvent(correlationId, 321, "session-id")))
            };

            message.SetEnclosedMessageType(typeof(NotifyEvent));

            var result = parser.Read<NotifyEvent>(message) as NotifyEvent;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyEvent));
            result.CorrelationId.ShouldBe(correlationId);
            result.SessionId.ShouldBe("session-id");
        }
    }
}

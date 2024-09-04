#if (EventPublisherEventHub)
using System;
using System.Text;
using Azure.Messaging.EventHubs;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Serialization;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.UnitTests;

public class JsonMessageSerializerTests
{
    [Fact]
    public void GivenTheParametersCorrectTheBodyOfTheApplicationEventWillBeParsed()
    {
        // Arrange
        var parser = new JsonMessageSerializer();
        var correlationId = Guid.NewGuid();
        var menuCreatedEvent = new DummyEventAes(101, correlationId);
        var jsonString = JsonConvert.SerializeObject(menuCreatedEvent);
        var byteArray = Encoding.UTF8.GetBytes(jsonString);
        var eventData = new EventData(byteArray);

        // Act
        var result = parser.Read<DummyEventAes>(eventData);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(DummyEventAes));
        result.CorrelationId.ShouldBe(correlationId);
    }
}
#endif

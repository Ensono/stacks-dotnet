using System;
using System.Text;
using Azure.Messaging.EventHubs;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Listener.Serialization;

namespace xxENSONOxx.xxSTACKSxx.Listener.UnitTests;

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
    
    [Fact]
    public void GivenInvalidJson_ReadShouldThrowJsonReaderException()
    {
        // Arrange
        var parser = new JsonMessageSerializer();
        var invalidJson = "Invalid JSON";
        var byteArray = Encoding.UTF8.GetBytes(invalidJson);
        var eventData = new EventData(byteArray);

        // Act & Assert
        Should.Throw<JsonReaderException>(() => parser.Read<DummyEventAes>(eventData));
    }
}

using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents;

[Trait("TestType", "UnitTests")]
public sealed class EventCodeUnitTests
{
    [Fact]
    public void EventCodes_Should_Have_Unique_Identifiers()
    {
        // Arrange
        var values = Enum.GetValues(typeof(EventCode)).Cast<int>().ToList();

        // Act
        var distinctValues = values.Distinct().ToList();

        // Assert
        values.Should().HaveSameCount(distinctValues, "All EventCode values should have unique identifiers.");
    }

    [Fact]
    public void Testing()
    {
        var autoFixture =  new Fixture().Customize(new AutoNSubstituteCustomization());
        var data = autoFixture.Create<MenuCreatedEvent>();
        var messageType = new StacksCloudEvent<MenuCreatedEvent>(data, data.CorrelationId);
        var type = messageType.GetType();
    }
}

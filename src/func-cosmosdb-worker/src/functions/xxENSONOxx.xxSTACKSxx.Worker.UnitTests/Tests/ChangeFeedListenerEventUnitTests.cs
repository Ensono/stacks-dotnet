using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.Worker.UnitTests.Tests;

public class ChangeFeedListenerEventUnitTests
{
    private readonly IFixture autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationCode()
    {
        // Arrange
        var operationCode = autoFixture.Create<int>();
        var correlationId = autoFixture.Create<Guid>();
        var entityId = autoFixture.Create<Guid>();
        var eTag = autoFixture.Create<string>();

        // Act
        var changeFeedEvent = new CosmosDbChangeFeedEvent(operationCode, correlationId, entityId, eTag);

        // Assert
        changeFeedEvent.EventCode.Should().Be((int)EventCode.EntityUpdated);
        changeFeedEvent.OperationCode.Should().Be(operationCode);
        changeFeedEvent.CorrelationId.Should().Be(correlationId);
        changeFeedEvent.EntityId.Should().Be(entityId);
        changeFeedEvent.ETag.Should().Be(eTag);
    }


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationContext()
    {
        // Arrange
        var context = autoFixture.Create<IOperationContext>();
        var entityId = autoFixture.Create<Guid>();
        var eTag = autoFixture.Create<string>();

        // Act
        var changeFeedEvent = new CosmosDbChangeFeedEvent(context, entityId, eTag);

        // Assert
        changeFeedEvent.EventCode.Should().Be((int)EventCode.EntityUpdated);
        changeFeedEvent.OperationCode.Should().Be(context.OperationCode);
        changeFeedEvent.CorrelationId.Should().Be(context.CorrelationId);
        changeFeedEvent.EntityId.Should().Be(entityId);
        changeFeedEvent.ETag.Should().Be(eTag);
    }
}

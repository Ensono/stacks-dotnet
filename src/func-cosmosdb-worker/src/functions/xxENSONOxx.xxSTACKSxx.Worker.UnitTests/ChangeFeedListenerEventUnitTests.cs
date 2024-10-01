using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.Worker.UnitTests;

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
        changeFeedEvent.Should().BeEquivalentTo(new
        {
            OperationCode = operationCode,
            CorrelationId = correlationId,
            EntityId = entityId,
            ETag = eTag,
            EventCode = (int)EventCode.EntityUpdated
        });
    }


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationContext()
    {
        // Arrange
        var operationCode = autoFixture.Create<int>();
        var correlationId = autoFixture.Create<Guid>();
        var context = autoFixture.Freeze<IOperationContext>();

        context.OperationCode.Returns(operationCode);
        context.CorrelationId.Returns(correlationId);

        var entityId = autoFixture.Create<Guid>();
        var eTag = autoFixture.Create<string>();

        // Act
        var changeFeedEvent = new CosmosDbChangeFeedEvent(context, entityId, eTag);

        // Assert
        changeFeedEvent.Should().BeEquivalentTo(new
        {
            OperationCode = operationCode,
            CorrelationId = correlationId,
            EntityId = entityId,
            ETag = eTag,
            EventCode = (int)EventCode.EntityUpdated
        });
    }
}

using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;

namespace xxENSONOxx.xxSTACKSxx.Worker.UnitTests;

public class ChangeFeedListenerEventUnitTests
{
    private readonly IFixture _autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationCode()
    {
        // Arrange
        var operationCode = _autoFixture.Create<int>();
        var correlationId = _autoFixture.Create<Guid>();
        var entityId = _autoFixture.Create<Guid>();
        var eTag = _autoFixture.Create<string>();

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
        var operationCode = _autoFixture.Create<int>();
        var correlationId = _autoFixture.Create<Guid>();
        var context = _autoFixture.Freeze<IOperationContext>();

        context.OperationCode.Returns(operationCode);
        context.CorrelationId.Returns(correlationId);

        var entityId = _autoFixture.Create<Guid>();
        var eTag = _autoFixture.Create<string>();

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

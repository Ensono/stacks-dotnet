using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Enums;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.Events;

[Trait("TestType", "UnitTests")]
public class MenuCreatedEventUnitTests
{
    private readonly IFixture autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationCode()
    {
        // Arrange
        var operationCode = autoFixture.Create<int>();
        var correlationId = autoFixture.Create<Guid>();
        var menuId = autoFixture.Create<Guid>();

        // Act
        var categoryCreatedEvent = new MenuCreatedEvent(operationCode, correlationId, menuId);

        // Assert
        categoryCreatedEvent.EventCode.Should().Be((int)EventCode.MenuCreated);
        categoryCreatedEvent.OperationCode.Should().Be(operationCode);
        categoryCreatedEvent.CorrelationId.Should().Be(correlationId);
        categoryCreatedEvent.MenuId.Should().Be(menuId);
    }


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationContext()
    {
        // Arrange
        var context = autoFixture.Create<IOperationContext>();
        var menuId = autoFixture.Create<Guid>();

        // Act
        var categoryCreatedEvent = new MenuCreatedEvent(context, menuId);

        // Assert
        categoryCreatedEvent.EventCode.Should().Be((int)EventCode.MenuCreated);
        categoryCreatedEvent.OperationCode.Should().Be(context.OperationCode);
        categoryCreatedEvent.CorrelationId.Should().Be(context.CorrelationId);
        categoryCreatedEvent.MenuId.Should().Be(menuId);
    }
}

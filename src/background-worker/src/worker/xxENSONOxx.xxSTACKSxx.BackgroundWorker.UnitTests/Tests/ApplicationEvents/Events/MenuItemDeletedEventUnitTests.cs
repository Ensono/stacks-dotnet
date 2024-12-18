using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Events;

[Trait("TestType", "UnitTests")]
public sealed class MenuItemDeletedEventUnitTests
{
    private readonly IFixture autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationCode()
    {
        // Arrange
        var operationCode = autoFixture.Create<int>();
        var correlationId = autoFixture.Create<Guid>();
        var menuId = autoFixture.Create<Guid>();
        var categoryId = autoFixture.Create<Guid>();
        var menuItemId = autoFixture.Create<Guid>();

        // Act
        var categoryCreatedEvent = new MenuItemDeletedEvent(operationCode, correlationId, menuId, categoryId, menuItemId);

        // Assert
        categoryCreatedEvent.EventCode.Should().Be((int)EventCode.MenuItemDeleted);
        categoryCreatedEvent.OperationCode.Should().Be(operationCode);
        categoryCreatedEvent.CorrelationId.Should().Be(correlationId);
        categoryCreatedEvent.MenuId.Should().Be(menuId);
        categoryCreatedEvent.CategoryId.Should().Be(categoryId);
        categoryCreatedEvent.MenuItemId.Should().Be(menuItemId);
    }


    [Fact]
    public void Constructor_ShouldAssignPropertiesCorrectly_WhenInitializedWithOperationContext()
    {
        // Arrange
        var context = autoFixture.Create<IOperationContext>();
        var menuId = autoFixture.Create<Guid>();
        var categoryId = autoFixture.Create<Guid>();
        var menuItemId = autoFixture.Create<Guid>();

        // Act
        var categoryCreatedEvent = new MenuItemDeletedEvent(context, menuId, categoryId, menuItemId);

        // Assert
        categoryCreatedEvent.EventCode.Should().Be((int)EventCode.MenuItemDeleted);
        categoryCreatedEvent.OperationCode.Should().Be(context.OperationCode);
        categoryCreatedEvent.CorrelationId.Should().Be(context.CorrelationId);
        categoryCreatedEvent.MenuId.Should().Be(menuId);
        categoryCreatedEvent.CategoryId.Should().Be(categoryId);
        categoryCreatedEvent.MenuItemId.Should().Be(menuItemId);
    }
}
